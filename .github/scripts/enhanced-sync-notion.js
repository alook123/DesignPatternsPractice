import fs from 'fs';
import path from 'path';
import { Client } from '@notionhq/client';
import { markdownToBlocks } from '@tryfabric/martian';
import { glob } from 'glob';

const notion = new Client({ auth: process.env.NOTION_TOKEN });
const databaseId = process.env.NOTION_DATABASE_ID;

// 收集所有 Markdown 文件
const patterns = ['./docs/**/*.md'];

const allFiles = [];
for (const pattern of patterns) {
    const files = glob.sync(pattern);
    allFiles.push(...files);
}

console.log(`🔍 Found ${allFiles.length} markdown files to sync`);

// 文档分类和元数据
const getDocumentMetadata = (filePath) => {
    const relativePath = path.relative('.', filePath);
    const fileName = path.basename(filePath, '.md');

    // 根据路径确定分类和属性
    if (relativePath.includes('Comparisons/')) {
        return {
            title: '创建型模式对比总结',
            category: '📚 总结对比',
            type: '对比文档',
            priority: 1,
            emoji: '📊',
        };
    }

    if (relativePath.includes('docs/CreationalPatterns/')) {
        // 从文件名推断模式名
        const patternName = fileName;
        return getCreationalPatternMetadata(patternName);
    }

    if (relativePath.includes('docs/StructuralPatterns/')) {
        // 从文件名推断模式名
        const patternName = fileName;
        return getStructuralPatternMetadata(patternName);
    }

    // 基础文档 (docs根目录)
    if (relativePath.startsWith('docs/') && !relativePath.includes('/')) {
        return {
            title: fileName.replace(/[-_]/g, ' '),
            category: '📖 基础文档',
            type: '指南文档',
            priority: 2,
            emoji: '📝',
        };
    }

    // 默认分类
    return {
        title: fileName.replace(/[-_]/g, ' '),
        category: '📄 其他文档',
        type: '文档',
        priority: 5,
        emoji: '�',
    };
};

// 创建型模式元数据
const getCreationalPatternMetadata = (patternName) => {
    const patterns = {
        SingletonPattern: {
            title: '单例模式 Singleton',
            emoji: '🔒',
        },
        FactoryMethodPattern: {
            title: '工厂方法模式 Factory Method',
            emoji: '🏭',
        },
        AbstractFactoryPattern: {
            title: '抽象工厂模式 Abstract Factory',
            emoji: '🏢',
        },
        BuilderPattern: {
            title: '建造者模式 Builder',
            emoji: '🔧',
        },
        PrototypePattern: {
            title: '原型模式 Prototype',
            emoji: '�',
        },
    };

    const pattern = patterns[patternName] || {
        title: patternName.replace(/Pattern/g, ''),
        emoji: '🏗️',
    };

    return {
        title: pattern.title,
        category: '🏗️ 创建型模式',
        type: '设计模式',
        priority: 3,
        emoji: pattern.emoji,
    };
};

// 结构型模式元数据
const getStructuralPatternMetadata = (patternName) => {
    const patterns = {
        AdapterPattern: {
            title: '适配器模式 Adapter',
            emoji: '�',
        },
        DecoratorPattern: {
            title: '装饰者模式 Decorator',
            emoji: '🎨',
        },
        FacadePattern: {
            title: '外观模式 Facade',
            emoji: '🏛️',
        },
    };

    const pattern = patterns[patternName] || {
        title: patternName.replace(/Pattern/g, ''),
        emoji: '🔧',
    };

    return {
        title: pattern.title,
        category: '� 结构型模式',
        type: '设计模式',
        priority: 4,
        emoji: pattern.emoji,
    };
};

// 处理每个文件
for (const filePath of allFiles) {
    try {
        const metadata = getDocumentMetadata(filePath);
        const markdown = fs.readFileSync(filePath, 'utf8');

        // 添加美化的标题
        const enhancedMarkdown = `# ${metadata.emoji} ${metadata.title}

${markdown.replace(/^#\s+.*$/m, '')}`;

        const blocks = markdownToBlocks(enhancedMarkdown);

        // 查找是否已存在同名文档
        let existing;
        try {
            existing = await notion.databases.query({
                database_id: databaseId,
                filter: {
                    property: 'Name',
                    title: {
                        equals: metadata.title,
                    },
                },
            });
        } catch (error) {
            console.log(
                `⚠️ Query error for ${metadata.title}: ${error.message}`
            );
            console.log('📝 Creating new page instead...');
            // 如果查询失败，尝试创建新页面
            existing = { results: [] };
        }

        const properties = {
            Name: {
                title: [
                    {
                        text: { content: metadata.title },
                    },
                ],
            },
            Category: {
                select: { name: metadata.category },
            },
            Type: {
                select: { name: metadata.type },
            },
            Priority: {
                number: metadata.priority,
            },
            Emoji: {
                rich_text: [
                    {
                        text: { content: metadata.emoji },
                    },
                ],
            },
            'Last Updated': {
                date: { start: new Date().toISOString() },
            },
            'File Path': {
                rich_text: [
                    {
                        text: { content: path.relative('.', filePath) },
                    },
                ],
            },
        };

        if (existing.results.length > 0) {
            const pageId = existing.results[0].id;
            console.log(`📝 Updating: ${metadata.emoji} ${metadata.title}`);

            // 更新页面属性
            await notion.pages.update({
                page_id: pageId,
                properties,
            });

            // 获取现有内容并删除
            const children = await notion.blocks.children.list({
                block_id: pageId,
                page_size: 100,
            });

            // 分批删除内容块
            for (const block of children.results) {
                try {
                    await notion.blocks.delete({ block_id: block.id });
                } catch (error) {
                    console.log(`⚠️ Could not delete block: ${error.message}`);
                }
            }

            // 添加新内容
            if (blocks.length > 0) {
                // Notion API 限制每次最多 100 个块
                const chunkSize = 100;
                for (let i = 0; i < blocks.length; i += chunkSize) {
                    const chunk = blocks.slice(i, i + chunkSize);
                    await notion.blocks.children.append({
                        block_id: pageId,
                        children: chunk,
                    });
                }
            }
        } else {
            console.log(`📄 Creating: ${metadata.emoji} ${metadata.title}`);

            const newPage = await notion.pages.create({
                parent: { database_id: databaseId },
                properties,
                children: blocks.slice(0, 100), // Notion 创建时限制 100 个块
            });

            // 如果有更多内容，分批添加
            if (blocks.length > 100) {
                for (let i = 100; i < blocks.length; i += 100) {
                    const chunk = blocks.slice(i, i + 100);
                    await notion.blocks.children.append({
                        block_id: newPage.id,
                        children: chunk,
                    });
                }
            }
        }

        // 避免 API 限制
        await new Promise((resolve) => setTimeout(resolve, 300));
    } catch (error) {
        console.error(`❌ Error processing ${filePath}:`, error.message);
    }
}

console.log('✅ Notion sync completed!');
