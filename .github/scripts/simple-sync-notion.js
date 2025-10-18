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

console.log(`🔑 Using database ID: ${process.env.NOTION_DATABASE_ID}`);
console.log(`🎫 Token length: ${process.env.NOTION_TOKEN ? process.env.NOTION_TOKEN.length : 'undefined'} chars`);

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

        // 先检查数据库结构
        const database = await notion.databases.retrieve({ 
            database_id: databaseId 
        });
        
        console.log(`🔍 Database retrieved:`, JSON.stringify(database, null, 2).substring(0, 500));
        
        // 检查 properties 是否存在
        if (!database || !database.properties) {
            throw new Error(`Database properties not found. Database object: ${JSON.stringify(database)}`);
        }
        
        // 找到标题属性
        const titleProperty = Object.entries(database.properties)
            .find(([key, prop]) => prop.type === 'title');
        
        if (!titleProperty) {
            console.log('Available properties:', Object.keys(database.properties));
            throw new Error('No title property found in database');
        }
        
        const titlePropertyName = titleProperty[0];
        console.log(`📝 Using title property: ${titlePropertyName}`);
        
        // 创建页面属性
        const properties = {};
        properties[titlePropertyName] = {
            title: [
                {
                    text: { content: title },
                },
            ],
        };

        // 直接创建页面，不查询现有页面
        const newPage = await notion.pages.create({
            parent: { database_id: databaseId },
            properties: properties,
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
