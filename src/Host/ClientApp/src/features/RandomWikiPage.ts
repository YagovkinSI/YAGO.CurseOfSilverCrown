export interface WikiPage {
  type: string;
  id: string;
}

export const WIKI_PAGES: WikiPage[] = [
  { type: 'ship', id: '1' },
  { type: 'ship', id: '2' },
  { type: 'lore', id: '1' },
  { type: 'lore', id: '2' },
  { type: 'lore', id: '3' },
  { type: 'lore', id: '4' },
  { type: 'lore', id: '5' },
  { type: 'lore', id: '6' },
  { type: 'lore', id: '7' },
  { type: 'lore', id: '8' },
  { type: 'parameters', id: '1' },
];

export const getRandomWikiPage = (): string => {
  const randomIndex = Math.floor(Math.random() * WIKI_PAGES.length);
  const randomPage = WIKI_PAGES[randomIndex];
  return `/wiki/${randomPage.type}/${randomPage.id}`;
};