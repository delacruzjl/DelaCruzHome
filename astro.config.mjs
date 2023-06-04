import { defineConfig } from 'astro/config';
import storyblok from "@storyblok/astro";
import { loadEnv } from 'vite';
import react from "@astrojs/react";
import sitemap from "@astrojs/sitemap";
const env = loadEnv("", process.cwd(), 'STORYBLOK');


// https://astro.build/config
export default defineConfig({
  site: 'https://delacruzhome.com',
  compressHTML: true,
  integrations: [react(), storyblok({
    accessToken: env.STORYBLOK_TOKEN,
    apiOptions: {
      region: 'us'
    }
  }), sitemap()]
});