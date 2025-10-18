import fs from 'fs';
import path from 'path';
import { Client } from '@notionhq/client';
import { markdownToBlocks } from '@tryfabric/martian';
import { glob } from 'glob';

const notion = new Client({ auth: process.env.NOTION_TOKEN });
const databaseId = process.env.NOTION_DATABASE_ID;

console.log('🚀 Starting Notion sync...');

// 检查必要的环境变量
if (!process.env.NOTION_TOKEN) {
    console.error('❌ NOTION_TOKEN not found');
    process.exit(1);
}

if (!process.env.NOTION_DATABASE_ID) {
    console.error('❌ NOTION_DATABASE_ID not found');
    process.exit(1);
}

// 收集文档
const files = glob.sync('./docs/**/*.md');
console.log(`🔍 Found ${files.length} markdown files`);

// 简化的文档处理
for (const filePath of files.slice(0, 2)) {
    // 只处理前2个文件进行测试
    try {
        const fileName = path.basename(filePath, '.md');
        const markdown = fs.readFileSync(filePath, 'utf8');

        console.log(`📄 Processing: ${fileName}`);

        // 简单的标题
        const title = fileName.replace(/[-_]/g, ' ');

        // 转换为 Notion 块
        const blocks = markdownToBlocks(markdown).slice(0, 50); // 限制块数量

        // 直接创建页面，不查询现有页面
        const newPage = await notion.pages.create({
            parent: { database_id: databaseId },
            properties: {
                Name: {
                    title: [
                        {
                            text: { content: title },
                        },
                    ],
                },
            },
            children: blocks,
        });

        console.log(`✅ Created: ${title}`);

        // 避免 API 限制
        await new Promise((resolve) => setTimeout(resolve, 1000));
    } catch (error) {
        console.error(`❌ Error processing ${filePath}:`, error.message);
    }
}

console.log('✅ Notion sync test completed!');
