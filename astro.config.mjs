import { defineConfig } from 'astro/config';
import storyblok from "@storyblok/astro";
import { loadEnv } from 'vite';
import react from "@astrojs/react";
const env = loadEnv("", process.cwd(), 'STORYBLOK');


// https://astro.build/config
export default defineConfig({
  integrations: [react(), storyblok({
    accessToken: env.STORYBLOK_TOKEN,
    apiOptions: {
      region: 'us'
    }
  })]
});