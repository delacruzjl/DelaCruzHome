import { renderRichText } from '@storyblok/astro';
import PostResultList from '../components/PostResultList.astro'

export default async function SearchPage() {

    const baseUrl = "https://api-us.storyblok.com/v2/cdn"
    const apiToken = `token=EaGcEhDCNHI8hXBPTeSsZQtt`
    const responseVersion = 'version=published'
    const filterQuery = "filter_query[component][in]=post"
    let pageUrl = `${baseUrl}/stories?${apiToken}&${responseVersion}&${filterQuery}`;

    // if (window.location.href["search"] && window.location.href["search"].length > 0) {    
    //     const urlSearchParams = new URLSearchParams([].concat(window.location.href.search))
    //     const params = Object.fromEntries(urlSearchParams.entries())
    //     const search = params["search"]

    //     if (search) {
    //         pageUrl = `${pageUrl}&search_term=${params["search"]}`
    //     }
    // }

    console.log(pageUrl)

    const response = await fetch(pageUrl);
    const blok = await response.json();

    const posts = blok?.stories?.sort((a: any, b: any) => a.published_at.valueOf() - b.published_at.valueOf())
    .map((story: any) => {
    const { published_at, full_slug } = story;
    const content_data = story.content;
    const { categories, images, summary, tag, title, content } = content_data;

    return {
        author: "Jose",
        categories: categories.map((cat: any) => ({ name: cat.name, url: `/category/${cat.name}` })),
        date: new Date(published_at),
        images: images.map((image: any) => image.filename),
        summary,
        tags: tag.map((tag: any) => ({ name: tag.name, url: `/tag/${tag.name}` })),
        title,
        url: `/posts/${full_slug}`,
        content: renderRichText(content)
    };
    });

   return (
    posts?.map((post: any) => (
        <PostResultList 
          author={post.author}
          categories={post.categories}
          postDate={post.date}
          postSummary={post.summary}
          tags={post.tags}
          postUrl={post.url}
          postTitle={post.title}
          images={post.images}
        />
      ))
   )
}