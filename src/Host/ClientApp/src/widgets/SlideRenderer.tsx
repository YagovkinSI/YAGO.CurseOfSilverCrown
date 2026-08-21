import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { X } from 'lucide-react';
import { FlexContainer } from '../shared/ui/FlexContainer';
import PageHeader, { type PageHeaderButton } from '../features/PageHeader';
import SlideContent from './slideRenderer/SlideContent';
import SlideBottomPanel from './slideRenderer/SlideBottomPanel';
import type { Slide, SlideButton } from '../entities/events/colonyEvent.types';
import Surface from '../shared/ui/Surface';

export interface SlideActions {
    onButtonClick?: (button: SlideButton) => void;
    onInfoSlideClick?: (slideId: string) => void;
    onSlideChange?: (slideId: string) => void;
}

export interface SlideInputState {
    value: string;
    error?: string;
    onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

export interface SlideHeaderOptions {
    leftButton?: PageHeaderButton;
    canBeClosed?: boolean;
}

interface SlideRendererProps {
    slide: Slide;
    actions?: SlideActions;
    inputState?: SlideInputState;
    header?: SlideHeaderOptions;
    renderBottomSlot?: React.ReactNode;
    createdAt?: string;
    resetScrollTrigger?: number | string;
}

const SlideRenderer: React.FC<SlideRendererProps> = ({
    slide,
    actions,
    inputState,
    header,
    renderBottomSlot,
    createdAt,
    resetScrollTrigger,
}) => {
    const navigate = useNavigate();
    const scrollContainerRef = React.useRef<HTMLDivElement>(null);

    useEffect(() => {
        if (scrollContainerRef.current) {
            scrollContainerRef.current.scrollTop = 0;
        }
    }, [resetScrollTrigger]);

    const renderHeader = () => {
        const rightButton = (header?.canBeClosed ?? true)
            ? { icon: X, onClick: () => navigate(-1), label: 'Закрыть' }
            : undefined;
        return (
            <div className="w-full sticky top-0 flex-shrink-0 z-20 border-b border-bright/10">
                <PageHeader
                    title={slide.title || 'Событие'}
                    leftButton={header?.leftButton}
                    rightButton={rightButton}
                />
            </div>
        );
    };

    const renderContent = () => (

        <Surface rounded='md' variant='default' className='mb-2 flex-1 gap-2 overflow-y-auto'
            ref={scrollContainerRef}
        >
            <SlideContent slide={slide} />
        </Surface>
    );

    const renderBottom = () => (
        <div className="w-full sticky bottom-0 flex-shrink-0 z-20 border-t border-bright/10">
            <SlideBottomPanel
                buttons={slide.buttons}
                actions={actions}
                inputState={inputState}
                renderBottomSlot={renderBottomSlot}
                createdAt={createdAt}
            />
        </div>
    );

    return (
        <FlexContainer className='h-full max-w-3xl mx-auto py-4 px-2 md:px-4 pb-2 md:pb-4'>
            {renderHeader()}
            {renderContent()}
            {renderBottom()}
        </FlexContainer>
    );
};

export default SlideRenderer;