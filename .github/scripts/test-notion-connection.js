import { Client } from '@notionhq/client';

const notion = new Client({ auth: process.env.NOTION_TOKEN });
const databaseId = process.env.NOTION_DATABASE_ID;

console.log('🧪 Testing Notion database connection...');

// 检查必要的环境变量
if (!process.env.NOTION_TOKEN) {
    console.error('❌ NOTION_TOKEN not found');
    process.exit(1);
}

if (!process.env.NOTION_DATABASE_ID) {
    console.error('❌ NOTION_DATABASE_ID not found');
    process.exit(1);
}

console.log(`🔑 Database ID: ${databaseId}`);
console.log(
    `🎫 Token starts with: ${process.env.NOTION_TOKEN.substring(0, 10)}...`
);

try {
    // 测试数据库访问
    console.log('📡 Attempting to retrieve database...');
    const database = await notion.databases.retrieve({
        database_id: databaseId,
    });

    console.log('✅ Database retrieved successfully!');
    console.log('📊 Database info:');
    console.log('- Title:', database.title?.[0]?.plain_text || 'No title');
    console.log('- Created:', database.created_time);
    console.log(
        '- Properties count:',
        Object.keys(database.properties || {}).length
    );

    if (database.properties) {
        console.log('🔑 Properties:');
        Object.entries(database.properties).forEach(([key, prop]) => {
            console.log(
                `  - ${key}: ${prop.type}${
                    prop.type === 'title' ? ' (TITLE)' : ''
                }`
            );
        });
    } else {
        console.log('⚠️  No properties found!');
    }

    // 测试查询权限
    console.log('\n🔍 Testing query permissions...');
    const queryResult = await notion.databases.query({
        database_id: databaseId,
        page_size: 1,
    });

    console.log(
        `✅ Query successful! Found ${queryResult.results.length} existing pages.`
    );
} catch (error) {
    console.error('❌ Error:', error.message);
    console.error('Error code:', error.code);
    console.error('Full error:', JSON.stringify(error, null, 2));
}
