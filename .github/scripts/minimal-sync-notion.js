import fs from 'fs';
import path from 'path';
import { Client } from '@notionhq/client';
import { markdownToBlocks } from '@tryfabric/martian';
import { glob } from 'glob';

const notion = new Client({ auth: process.env.NOTION_TOKEN });
const databaseId = process.env.NOTION_DATABASE_ID;

console.log('🚀 Starting minimal Notion sync...');

// 检查必要的环境变量
if (!process.env.NOTION_TOKEN || !process.env.NOTION_DATABASE_ID) {
    console.error('❌ Missing NOTION_TOKEN or NOTION_DATABASE_ID');
    process.exit(1);
}

// 收集文档
const files = glob.sync('./docs/**/*.md');
console.log(`🔍 Found ${files.length} markdown files`);

// 只处理前2个文件进行测试
for (const filePath of files.slice(0, 2)) {
    try {
        const fileName = path.basename(filePath, '.md');
        const markdown = fs.readFileSync(filePath, 'utf8');

        console.log(`📄 Processing: ${fileName}`);

        // 简单的标题
        const title = fileName.replace(/[-_]/g, ' ');

        // 转换为 Notion 块，限制数量避免超时
        const blocks = markdownToBlocks(markdown).slice(0, 20);

        // 确定分类
        let category = 'Documentation';
        if (fileName.includes('Factory') || fileName.includes('Singleton') || 
            fileName.includes('Builder') || fileName.includes('Prototype')) {
            category = 'Creational Pattern';
        } else if (fileName.includes('Adapter')) {
            category = 'Structural Pattern';
        } else if (fileName.includes('Comparison')) {
            category = 'Comparison';
        }

        // 创建页面，假设数据库有标准属性
        const pageProperties = {
            'Title': {
                title: [{ text: { content: title } }]
            },
            'Category': {
                select: { name: category }
            },
            'Status': {
                select: { name: 'Complete' }
            }
        };

        console.log(`🔨 Creating page with properties:`, Object.keys(pageProperties));

        const newPage = await notion.pages.create({
            parent: { database_id: databaseId },
            properties: pageProperties,
            children: blocks,
        });

        console.log(`✅ Created: ${title}`);

        // 避免 API 限制
        await new Promise((resolve) => setTimeout(resolve, 2000));
        
    } catch (error) {
        console.error(`❌ Error processing ${filePath}:`, error.message);
        
        // 提供更详细的错误信息
        if (error.code === 'validation_error') {
            console.error('💡 This might be a property name mismatch.');
            console.error('   Make sure the database has the correct properties.');
        }
    }
}

console.log('✅ Minimal Notion sync completed!');