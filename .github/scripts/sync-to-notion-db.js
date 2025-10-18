import fs from 'fs';
import path from 'path';
import { Client } from '@notionhq/client';
import { markdownToBlocks } from '@tryfabric/martian';

const notion = new Client({ auth: process.env.NOTION_TOKEN });
const databaseId = process.env.NOTION_DATABASE_ID;

const docsDir = './docs';
const files = fs.readdirSync(docsDir).filter((f) => f.endsWith('.md'));

for (const file of files) {
    const title = path.basename(file, '.md');
    const markdown = fs.readFileSync(path.join(docsDir, file), 'utf8');
    const blocks = markdownToBlocks(markdown);

    // 查找是否已存在同名文档
    const existing = await notion.databases.query({
        database_id: databaseId,
        filter: { property: 'Name', title: { equals: title } },
    });

    if (existing.results.length > 0) {
        const pageId = existing.results[0].id;
        console.log(`📝 Updating: ${title}`);

        // 删除旧内容（保留标题等属性）
        const children = await notion.blocks.children.list({
            block_id: pageId,
        });
        for (const block of children.results) {
            await notion.blocks.delete({ block_id: block.id });
        }

        // 添加新内容
        await notion.blocks.children.append({
            block_id: pageId,
            children: blocks,
        });
    } else {
        console.log(`📄 Creating: ${title}`);
        await notion.pages.create({
            parent: { database_id: databaseId },
            properties: {
                Name: { title: [{ text: { content: title } }] },
                Updated: { date: { start: new Date().toISOString() } },
            },
            children: blocks,
        });
    }
}
