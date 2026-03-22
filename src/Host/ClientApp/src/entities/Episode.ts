import type { Slide } from "./Slide";


export interface Episode {
    id: string | undefined;
    prologSlides: Slide[];
    choice: Slide[];
    choiceLabel: string | undefined;
    isCycleCompleted: boolean;
}
