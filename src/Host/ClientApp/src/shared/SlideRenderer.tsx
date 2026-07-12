import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { HelpCircle, Clock, X } from 'lucide-react';
import type { Slide, SlideButton } from '../entities/Episode';
import type { ColonyParameter } from '../entities/ColonyParameter';
import Button from './Button';
import InputText from './InputText';
import Text from './Text';
import ColonyParameterRowList from '../features/ColonyParameterList';
import { FlexContainer } from './FlexContainer';
import PageHeader, { type PageHeaderButton } from '../features/PageHeader';

interface SlideRendererProps {
    slide: Slide;
    title?: string;
    inputTextValue?: string;
    inputTextError?: string;
    hasTextInput?: boolean;
    onInputTextChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
    onButtonClick: (button: SlideButton, textValue?: string) => void;
    onInfoSlideClick: (slideId: string) => void;
    onNavigate?: (url: string) => void;
    onSlideChange?: (slideId: string) => void;
    renderBottomSlot?: React.ReactNode;
    createdAt?: string;
    canBeClosed?: boolean;
    onClose?: () => void;
    leftButton?: PageHeaderButton;
    resetScrollTrigger?: number | string;
}

const SlideRenderer: React.FC<SlideRendererProps> = ({
    slide,
    title,
    inputTextValue,
    inputTextError,
    hasTextInput,
    onInputTextChange,
    onButtonClick,
    onInfoSlideClick,
    onNavigate,
    onSlideChange,
    renderBottomSlot,
    createdAt,
    canBeClosed = true,
    onClose,
    leftButton,
    resetScrollTrigger,
}) => {
    const navigate = useNavigate();
    const scrollContainerRef = React.useRef<HTMLDivElement>(null);

    useEffect(() => {
        if (scrollContainerRef.current) {
            scrollContainerRef.current.scrollTop = 0;
        }
    }, [resetScrollTrigger]);

    const renderParameters = (parameters: ColonyParameter[]) => {
        if (!parameters || parameters.length === 0) return null;
        return (
            <div className="w-full">
                <ColonyParameterRowList items={parameters ?? []} dense={true} />
            </div>
        );
    };

    const renderButtons = () => {
        const buttons = slide?.buttons || [];

        return (
            <div className="flex flex-col gap-2 w-full">
                {buttons.map((button: SlideButton, index: number) => {
                    const isMutation = button.action != undefined;

                    const handleClick = () => {
                        if (button.action) {
                            if (hasTextInput) {
                                onButtonClick(button, inputTextValue);
                            } else {
                                onButtonClick(button);
                            }
                        } else if (button.navigate) {
                            if (onNavigate) {
                                onNavigate(button.navigate.actionUrl);
                            } else {
                                navigate(button.navigate.actionUrl);
                            }
                        } else if (button.toSlide && onSlideChange) {
                            onSlideChange(button.toSlide.slideId);
                        }
                    };

                    return (
                        <div key={index} className="flex items-center gap-2">
                            <Button
                                variant={isMutation ? 'primary' : 'secondary'}
                                size="sm"
                                sizeMd="md"
                                onClick={handleClick}
                                disabled={!button.isAvailable}
                                className="flex-1"
                            >
                                {button.name}
                            </Button>
                            {button.infoSlideId && (
                                <button
                                    onClick={() => onInfoSlideClick(button.infoSlideId!)}
                                    className="flex-shrink-0 w-10 h-10 rounded-lg border border-bright/20 
                                        text-muted hover:text-light hover:border-bright/40 transition-colors flex items-center justify-center"
                                    aria-label="Подробнее"
                                >
                                    <HelpCircle className="w-4 h-4" />
                                </button>
                            )}
                        </div>
                    );
                })}
            </div>
        );
    };

    const renderBottomPanel = () => {
        return (
            <div className="flex flex-col transition-all duration-300 flex-shrink-0 mt-2 overflow-hidden">
                <div className="flex-shrink-0 py-2 space-y-2 mx-auto w-full">
                    {hasTextInput && (
                        <InputText
                            name="slideInputText"
                            label="Название колонии"
                            type="text"
                            value={inputTextValue ?? ''}
                            handleChange={onInputTextChange}
                            handleBlur={onInputTextChange}
                            error={!!inputTextError}
                            helperText={inputTextError}
                        />
                    )}

                    {renderButtons()}

                    {renderBottomSlot}

                    {canBeClosed && onClose && (
                        <Button variant="secondary" size="sm" onClick={onClose}>
                            Закрыть
                        </Button>
                    )}

                    {createdAt && (
                        <div className="flex items-center gap-2 pt-1">
                            <Clock className="w-3 h-3 text-muted/30" />
                            <span className="text-[0.55rem] text-muted/30">
                                {createdAt}
                            </span>
                        </div>
                    )}
                </div>
            </div>
        );
    };

    const renderCentralPart = () => {
        return (
            <div className="min-h-full w-full max-w-5xl mx-auto bg-dark/40 backdrop-blur-sm border border-bright/5">
                <div className="relative w-full overflow-hidden">
                    <img
                        src={`/images/pictures/${slide?.imageName}.jpg`}
                        alt={title || 'Иллюстрация'}
                        className="w-full h-auto object-cover object-center"
                    />
                    <div className="absolute inset-0 bg-gradient-to-t from-dark/80 via-dark/20 to-transparent pointer-events-none" />
                </div>

                <div className="px-4">
                    <div className="space-y-2 w-full">
                        {slide?.text.map((item, index) => (
                            <Text key={index} size="sm" align='left' className="leading-relaxed">
                                {item}
                            </Text>
                        ))}
                    </div>
                    {renderParameters(slide?.parameters ?? [])}
                </div>
            </div>
        );
    };

    return (
        <FlexContainer className='h-full max-w-5xl mx-auto py-4 px-2 md:px-4 pb-2 md:pb-4'>
            <div className="w-full sticky top-0 flex-shrink-0 z-20 border-b border-bright/10">
                <PageHeader
                    title={title || 'Событие'}
                    leftButton={leftButton}
                    rightButton={{ icon: X, onClick: () => navigate(-1), label: 'Закрыть' }}
                />
            </div>

            <div
                ref={scrollContainerRef}
                className="flex-1 w-full overflow-y-auto scrollbar-hide z-10 relative"
            >
                {renderCentralPart()}
            </div>

            <div className="w-full sticky bottom-0 flex-shrink-0 z-20 border-t border-bright/10">
                {renderBottomPanel()}
            </div>
        </FlexContainer>
    );
};

export default SlideRenderer;