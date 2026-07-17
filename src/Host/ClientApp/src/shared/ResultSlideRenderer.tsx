import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Clock } from 'lucide-react';
import type { Slide } from '../entities/Episode';
import type { ColonyParameter } from '../entities/ColonyParameter';
import Button from './Button';
import ColonyParameterRowList from '../features/ColonyParameterList';
import Text from './Text';
import { FlexContainer } from './FlexContainer';
import Card from './Card';
import Title from './Title';

interface ResultSlideRendererProps {
    slide: Slide;
    title?: string;
    createdAt?: string;
    onClose?: () => void;
}

const ResultSlideRenderer: React.FC<ResultSlideRendererProps> = ({
    slide,
    onClose,
    createdAt,
}) => {
    const navigate = useNavigate();

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

    const renderContent = () => (
        <div className="w-full mx-auto">
            {slide?.imageName && (
                <div className="relative w-full overflow-hidden">
                    <img
                        src={`/images/pictures/${slide.imageName}.jpg`}
                        alt={slide.title || 'Иллюстрация'}
                        className="w-full h-full object-cover object-center"
                    />
                    <div className="absolute inset-0 bg-gradient-to-t from-dark/80 via-dark/20 to-transparent pointer-events-none" />
                </div>
            )}

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
        </div>)

    const renderButtons = () => (
        <Button onClick={handleClose}
        >
            Закрыть
        </Button>)

    const renderFooter = () => {
        return createdAt && (
            <div className="flex items-center justify-center gap-2 pt-2">
                <Clock className="w-3 h-3 text-muted/30" />
                <span className="text-[0.55rem] text-muted/30">
                    {createdAt}
                </span>
            </div>
        )
    }

    return (
        <div className='h-full overflow-y-auto scrollbar-hide'>
            <FlexContainer className='p-2'>
                <Card variant="glow" className="w-full flex flex-col items-center">
                    <Title>{slide.title}</Title>
                    {renderContent()}
                    {renderButtons()}
                    {renderFooter()}
                </Card>
            </FlexContainer>
        </div>
    )
};

export default ResultSlideRenderer;