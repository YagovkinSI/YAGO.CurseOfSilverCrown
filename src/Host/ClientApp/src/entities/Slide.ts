export interface Slide {
    id: number,
    title: string,
    imageName: string,
    text: string[],
    footer?: string | undefined
}