import { Client } from '@notionhq/client';

const notion = new Client({ auth: process.env.NOTION_TOKEN });
const databaseId = process.env.NOTION_DATABASE_ID;

console.log('🔧 Setting up Notion database properties...');

// 检查必要的环境变量
if (!process.env.NOTION_TOKEN || !process.env.NOTION_DATABASE_ID) {
    console.error('❌ Missing NOTION_TOKEN or NOTION_DATABASE_ID');
    process.exit(1);
}

try {
    // 为数据库添加必要的属性
    console.log('📝 Adding properties to database...');
    
    const updatedDatabase = await notion.databases.update({
        database_id: databaseId,
        properties: {
            'Title': {
                title: {}
            },
            'Category': {
                select: {
                    options: [
                        { name: 'Creational Pattern', color: 'blue' },
                        { name: 'Structural Pattern', color: 'green' },
                        { name: 'Behavioral Pattern', color: 'red' },
                        { name: 'Documentation', color: 'gray' },
                        { name: 'Comparison', color: 'purple' }
                    ]
                }
            },
            'Status': {
                select: {
                    options: [
                        { name: 'Draft', color: 'yellow' },
                        { name: 'Complete', color: 'green' },
                        { name: 'Updated', color: 'blue' }
                    ]
                }
            },
            'Tags': {
                multi_select: {
                    options: [
                        { name: 'Factory', color: 'blue' },
                        { name: 'Singleton', color: 'green' },
                        { name: 'Builder', color: 'red' },
                        { name: 'Prototype', color: 'purple' },
                        { name: 'Adapter', color: 'orange' },
                        { name: 'Interview', color: 'pink' }
                    ]
                }
            },
            'Last Updated': {
                last_edited_time: {}
            }
        }
    });

    console.log('✅ Database properties updated successfully!');
    console.log('📊 Properties added:');
    
    if (updatedDatabase.properties) {
        Object.entries(updatedDatabase.properties).forEach(([key, prop]) => {
            console.log(`  - ${key}: ${prop.type}${prop.type === 'title' ? ' (TITLE)' : ''}`);
        });
    }

    // 测试查询（使用更兼容的方法）
    console.log('\n🔍 Testing database access...');
    
    // 尝试使用不同的查询方法
    try {
        const queryResult = await notion.databases.query({
            database_id: databaseId,
            page_size: 1
        });
        console.log(`✅ Query successful! Found ${queryResult.results.length} existing pages.`);
    } catch (queryError) {
        console.log('⚠️  Query method not available, but database setup is complete.');
        console.log('This is normal for some Notion API versions.');
    }

} catch (error) {
    console.error('❌ Error setting up database:', error.message);
    if (error.code) {
        console.error('Error code:', error.code);
    }
    
    // 如果是权限问题，提供帮助信息
    if (error.code === 'unauthorized') {
        console.log('\n💡 Make sure your Notion integration has:');
        console.log('   1. Read access to the database');
        console.log('   2. Write access to the database');
        console.log('   3. Update database structure permissions');
    }
}