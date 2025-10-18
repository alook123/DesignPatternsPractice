import fs from 'fs';
import path from 'path';
import { Client } from '@notionhq/client';
import { markdownToBlocks } from '@tryfabric/martian';

const notion = new Client({ auth: process.env.NOTION_TOKEN });
const docsDir = './docs';
const parentPageId = process.env.NOTION_PARENT_PAGE_ID;

async function uploadDoc(file) {
    const title = path.basename(file, '.md');
    const markdown = fs.readFileSync(path.join(docsDir, file), 'utf8');
    const blocks = markdownToBlocks(markdown);

    // 查找同名页面
    const search = await notion.search({ query: title });
    const existing = search.results.find(
        (p) =>
            p.object === 'page' &&
            p.properties?.title?.title?.[0]?.plain_text === title
    );

    if (existing) {
        console.log(`📝 Updating: ${title}`);
        // 先清空旧内容（Notion API 不支持直接替换，只能追加或删除）
        // 这里简单做法：追加到后面
        await notion.blocks.children.append({
            block_id: existing.id,
            children: blocks,
        });
    } else {
        console.log(`📘 Creating: ${title}`);
        await notion.pages.create({
            parent: { page_id: parentPageId },
            properties: {
                title: { title: [{ text: { content: title } }] },
            },
            children: blocks,
        });
    }
}

async function main() {
    const files = fs.readdirSync(docsDir).filter((f) => f.endsWith('.md'));
    for (const f of files) {
        await uploadDoc(f);
    }
    console.log('✅ Sync completed!');
}

main().catch(console.error);
