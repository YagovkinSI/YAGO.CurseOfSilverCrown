export interface WikiPage {
  type: string;
  id: string;
}

export const WIKI_PAGES: WikiPage[] = [
  { type: 'ship', id: '1' },
  { type: 'lore', id: '1'  },
  { type: 'parameters', id: '1' },
];

export const getRandomWikiPage = (): string => {
  const randomIndex = Math.floor(Math.random() * WIKI_PAGES.length);
  const randomPage = WIKI_PAGES[randomIndex];
  return `/wiki/${randomPage.type}/${randomPage.id}`;
};