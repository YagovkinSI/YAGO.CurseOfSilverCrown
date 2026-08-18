import React from 'react';
import { useNavigate } from 'react-router-dom';
import Button from '../../shared/ui/buttons/Button';
import ColonyParameterRowList from '../../features/ColonyParameterList';
import Text from '../../shared/ui/Text';
import { FlexContainer } from '../../shared/ui/FlexContainer';
import Card from '../../shared/ui/Card';
import Title from '../../shared/ui/Title';
import type { ColonyParameter } from '../colonies/colony.types';
import type { EventResultSlide } from './colonyEvent.types';

interface ResultSlideRendererProps {
    eventResult: EventResultSlide;
    onClose?: () => void;
}

const ResultSlideRenderer: React.FC<ResultSlideRendererProps> = ({
    eventResult,
    onClose,
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
            {eventResult?.imageName && (
                <div className="relative w-full overflow-hidden">
                    <img
                        src={`/images/pictures/${eventResult.imageName}.jpg`}
                        alt={eventResult.title}
                        className="w-full h-full object-cover object-center"
                    />
                    <div className="absolute inset-0 bg-gradient-to-t from-dark/80 via-dark/20 to-transparent pointer-events-none" />
                </div>
            )}

            <div className="py-4 space-y-4">
                <div className="space-y-2">
                    {eventResult?.text?.map((item, index) => (
                        <Text key={index} size="sm" align="left" className="leading-relaxed">
                            {item}
                        </Text>
                    ))}
                </div>
                {renderParameters(eventResult?.parameters ?? [])}
            </div>
        </div>)

    const renderButtons = () => (
        <Button onClick={handleClose}
        >
            Закрыть
        </Button>)

    return (
        <div className='h-full overflow-y-auto scrollbar-hide'>
            <FlexContainer className='p-2'>
                <Card variant="glow" className="w-full flex flex-col items-center">
                    <Title>{eventResult.title}</Title>
                    {renderContent()}
                    {renderButtons()}
                </Card>
            </FlexContainer>
        </div>
    )
};

export default ResultSlideRenderer;