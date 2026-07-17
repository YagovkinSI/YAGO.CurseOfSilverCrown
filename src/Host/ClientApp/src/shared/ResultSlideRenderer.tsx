import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Clock, X, CheckCircle } from 'lucide-react';
import type { Slide } from '../entities/Episode';
import type { ColonyParameter } from '../entities/ColonyParameter';
import PageHeader from '../features/PageHeader';
import Button from './Button';
import ColonyParameterRowList from '../features/ColonyParameterList';
import Text from './Text';
import { FlexContainer } from './FlexContainer';

interface ResultSlideRendererProps {
    slide: Slide;
    title?: string;
    createdAt?: string;
    onClose?: () => void;
    resetScrollTrigger?: number | string;
}

const ResultSlideRenderer: React.FC<ResultSlideRendererProps> = ({
    slide,
    onClose,
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

    const renderParameters = (parameters: ColonyParameter[]) => {
        if (!parameters || parameters.length === 0) return null;
        return (
            <div className="w-full">
                <ColonyParameterRowList items={parameters ?? []} dense={true} />
            </div>
        );
    };

    const handleClose = () => {
        if (onClose) {
            onClose();
        } else {
            navigate(-1);
        }
    };

    const renderCentralPart = () => {
        return (
            <div className="min-h-full w-full max-w-3xl mx-auto">
                {/* Баннер "Итог" */}
                <div className="flex items-center justify-center gap-2 py-2 border-b border-bright/10">
                    <CheckCircle className="w-4 h-4 text-bright" />
                    <span className="text-xs font-medium text-bright uppercase tracking-wider">
                        Итог события
                    </span>
                </div>

                {/* Иллюстрация (опционально) */}
                {slide?.imageName && (
                    <div className="relative w-full overflow-hidden max-h-[40vh]">
                        <img
                            src={`/images/pictures/${slide.imageName}.jpg`}
                            alt={slide.title || 'Иллюстрация'}
                            className="w-full h-full object-cover object-center"
                        />
                        <div className="absolute inset-0 bg-gradient-to-t from-dark/80 via-dark/20 to-transparent pointer-events-none" />
                    </div>
                )}

                {/* Текст и параметры */}
                <div className="p-4 space-y-4">
                    <div className="space-y-2">
                        {slide?.text?.map((item, index) => (
                            <Text key={index} size="sm" align="left" className="leading-relaxed">
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
        <FlexContainer className="h-full max-w-3xl mx-auto py-4 px-2 md:px-4 pb-2 md:pb-4">
            {/* Хедер с кнопкой "Закрыть" */}
            <div className="w-full sticky top-0 flex-shrink-0 z-20 border-b border-bright/10 bg-dark/40 backdrop-blur-sm">
                <PageHeader
                    title={slide.title || 'Итог'}
                    leftButton={undefined}
                    rightButton={{
                        icon: X,
                        onClick: handleClose,
                        label: 'Закрыть',
                    }}
                />
            </div>

            {/* Контент */}
            <div
                ref={scrollContainerRef}
                className="flex-1 w-full overflow-y-auto scrollbar-hide z-10 relative"
            >
                {renderCentralPart()}
            </div>

            {/* Футер с кнопкой "Закрыть" (дублирует хедер для удобства) */}
            <div className="w-full sticky bottom-0 flex-shrink-0 z-20 border-t border-bright/10 bg-dark/40 backdrop-blur-sm py-3 px-4">
                <Button
                    variant="secondary"
                    sizeSm="sm"
                    sizeMd="sm"
                    onClick={handleClose}
                    className="max-w-md mx-auto"
                >
                    Закрыть
                </Button>
                {createdAt && (
                    <div className="flex items-center justify-center gap-2 pt-2">
                        <Clock className="w-3 h-3 text-muted/30" />
                        <span className="text-[0.55rem] text-muted/30">
                            {createdAt}
                        </span>
                    </div>
                )}
            </div>
        </FlexContainer>
    );
};

export default ResultSlideRenderer;