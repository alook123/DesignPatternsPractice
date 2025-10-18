import fs from 'fs';
import path from 'path';
import { Client } from '@notionhq/client';
import { markdownToBlocks } from '@tryfabric/martian'; // Markdown → Notion blocks

const notion = new Client({ auth: process.env.NOTION_TOKEN });
const docsDir = './docs';

const parentPageId = process.env.NOTION_PARENT_PAGE_ID;

async function uploadDoc(file) {
    const title = path.basename(file, '.md');
    const markdown = fs.readFileSync(path.join(docsDir, file), 'utf8');
    const blocks = markdownToBlocks(markdown);

    // 查找是否已存在页面
    const search = await notion.search({ query: title });
    const existing = search.results.find(
        (p) =>
            p.object === 'page' &&
            p.properties?.title?.title?.[0]?.plain_text === title
    );

    if (existing) {
        // 更新已有页面内容
        await notion.blocks.children.append({
            block_id: existing.id,
            children: blocks,
        });
        console.log(`✅ Updated: ${title}`);
    } else {
        // 创建新页面
        await notion.pages.create({
            parent: { page_id: parentPageId },
            properties: { title: { title: [{ text: { content: title } }] } },
            children: blocks,
        });
        console.log(`📘 Created: ${title}`);
    }
}

const files = fs.readdirSync(docsDir).filter((f) => f.endsWith('.md'));
for (const f of files) {
    await uploadDoc(f);
}
