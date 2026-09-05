export type CouncilPositionCode = 'administrator' | 'engineer' | 'financier' | 'social';

export interface CouncilMember {
    name: string;
    avatar: string;
    loyalty: number;
    wikiArticleCode: string;
}

export interface CouncilPosition {
    code: CouncilPositionCode;
    title: string;
    description: string;
    member: CouncilMember | null;
}