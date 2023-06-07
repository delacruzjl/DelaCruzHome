import { loadEnv } from 'vite';
import { renderRichText } from '@storyblok/astro';

export interface BlogPost {
    author: string,
    categories: Array<PostCategory>,
    date: Date,
    images: Array<string>,
    summary: string,
    tags: Array<PostTag>,
    title: string,
    url: string,
    slug: string,
    content: string
}

export interface PostCategory { 
    name: string, 
    count?: number, 
    url: string 
}

export interface PostTag {
    name: string, 
    url: string
}

export class PostService {

    getCategoryLinks(posts: Array<BlogPost>): Array<PostCategory> {
        if (!posts || posts.length === 0){
            return []
        } 

        const rawCategories = posts?.map((post: BlogPost) => 
            post.categories).flat()

        const categories: Array<PostCategory> = [] 
        rawCategories?.forEach((category: PostCategory) => {
            let idx = categories.findIndex((c) => c.name === category.name)

            if (idx >= 0) {
                categories[idx].count = categories[idx].count + 1 || 1
            } else {
                categories.push({name: category.name, count: 1, url: category.url})
            }
        })

        return categories
    }

    getTagLinks(posts: Array<BlogPost>): Array<PostTag> {
        if (!posts || posts.length === 0) {
            return []
        }

        return posts?.map((post: BlogPost) => 
                post.tags)
            .flat()
            .filter((tag: PostTag, index: number, self: any) => 
                self.findIndex((t: any) => t.name === tag.name) === index)
    }

    async getPosts(): Promise<BlogPost[]> {
        const env = loadEnv("", process.cwd(), 'STORYBLOK');

        const apiToken = `token=${env.STORYBLOK_TOKEN}`
        const responseVersion = 'version=published'
        const filterQuery = "filter_query[component][in]=post"

        const baseUrl = env.STORYBLOK_API_BASE_URL
        const pageUrl = `${baseUrl}/stories?${apiToken}&${responseVersion}&${filterQuery}`

        const response = await fetch(pageUrl);
        if (response.status >= 400) {
            return []
        }

        const blok = await response.json();

        const posts = blok?.stories?.sort((a: any, b: any) => 
                a.published_at.valueOf() - b.published_at.valueOf())
            .map((story: any) => {
                const { published_at, full_slug, tag_list } = story;
                const content_data = story.content;
                const { categories, images, summary, title, content } = content_data;

                return {
                    author: env.STORYBLOK_AUTHOR,
                    categories: categories?.map((cat: any) => {
                        return { name: cat?.name, url: `/category/${cat?.name?.toLowerCase()}` }}) || [],
                    date: new Date(published_at),
                    images: images.map((image: any) => 
                        image.filename),
                    summary,
                    tags: tag_list?.map((tag: string) => {
                        return { name: tag, url: `/tag/${tag?.toLowerCase()}` }}) || [],
                    title,
                    url: `/post/${full_slug}`,
                    slug: full_slug,
                    content: renderRichText(content)
                };
        });

        return posts;
    }

}