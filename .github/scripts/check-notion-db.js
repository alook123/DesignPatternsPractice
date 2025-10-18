import { Client } from '@notionhq/client';

const notion = new Client({ auth: process.env.NOTION_TOKEN });
const databaseId = process.env.NOTION_DATABASE_ID;

console.log('🔍 Checking Notion database structure...');

// 检查必要的环境变量
if (!process.env.NOTION_TOKEN) {
    console.error('❌ NOTION_TOKEN not found');
    process.exit(1);
}

if (!process.env.NOTION_DATABASE_ID) {
    console.error('❌ NOTION_DATABASE_ID not found');
    process.exit(1);
}

try {
    // 获取数据库信息
    const database = await notion.databases.retrieve({
        database_id: databaseId,
    });

    console.log('📊 Database Information:');
    console.log('Title:', database.title[0]?.plain_text || 'No title');
    console.log('\n🔑 Available Properties:');

    Object.entries(database.properties).forEach(([key, property]) => {
        console.log(`- ${key}: ${property.type}`);
        if (property.type === 'title') {
            console.log(`  👑 This is the title property`);
        }
    });

    // 尝试查询数据库（测试权限）
    console.log('\n🔐 Testing query permissions...');
    const queryResult = await notion.databases.query({
        database_id: databaseId,
        page_size: 1,
    });

    console.log(
        `✅ Query successful! Found ${queryResult.results.length} existing pages.`
    );
} catch (error) {
    console.error('❌ Error:', error.message);
    if (error.code) {
        console.error('Error Code:', error.code);
    }
}
