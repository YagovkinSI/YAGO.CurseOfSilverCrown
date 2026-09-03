export interface WikiArticle {
    code: string;
    name: string;
    image: string;
    text: string[];
}

export interface WikiSummary {
    code: string;
    name: string;
    isRead: boolean;
}
