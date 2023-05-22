import { defineConfig } from 'astro/config';
import storyblok from "@storyblok/astro";
import { loadEnv } from 'vite';

const env = loadEnv("", process.cwd(), 'STORYBLOK');

// https://astro.build/config
export default defineConfig({
    integrations: [
        storyblok({
          accessToken: env.STORYBLOK_TOKEN,
          apiOptions: {
            region: 'us'
          }
        }),
      ],
});
