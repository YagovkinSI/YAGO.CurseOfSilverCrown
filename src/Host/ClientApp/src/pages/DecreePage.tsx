import YagoSlide from '../shared/YagoSlide';
import YagoButton from '../shared/YagoButton';
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import SlideCard from '../features/SlideCard';
import { useGetMyColonyQuery, useIssueDecreeMutation } from '../entities/MyColony';
import { useGetDecreeQuery, type DecreeDetails } from '../entities/DecreeDetails';
import YagoCardContentSelection from '../shared/YagoCardContentSelection';
import YagoText from '../shared/YagoText';
import type { Slide } from '../entities/Episode';
import ColonyParameterList from '../features/ColonyParameterList';
import PageContainer from '../shared/PageContainer';

const DecreePage: React.FC = () => {
    const [decreeId, setDecreeId] = useState<number>(1);
    const [showSlide, setShowSlide] = useState<boolean>(false);
    const myColonyResult = useGetMyColonyQuery();
    const decreeResult = useGetDecreeQuery(decreeId);
    const [issueDecree, issueDecreeResult] = useIssueDecreeMutation();
    const navigate = useNavigate();

    const isLoading = decreeResult.isLoading || myColonyResult.isLoading || issueDecreeResult.isLoading;
    const error = decreeResult.error ?? myColonyResult.error ?? issueDecreeResult.error;
    const decreeIdMax = 3;

    useEffect(() => {
        if (myColonyResult.data != undefined && myColonyResult.data.data == undefined) {
            navigate('/');
        }
    }, [myColonyResult, navigate]);

    const handleNextDecree = () => {
        const nextIndex = (decreeId % decreeIdMax) + 1;
        setDecreeId(nextIndex);
    };

    const handlePrevDecree = () => {
        const prevIndex = decreeId == 1 ? decreeIdMax : decreeId - 1;
        setDecreeId(prevIndex);
    };

    const handleIssueDecree = async (decreeId: number) => {
        await issueDecree({ decreeId }).unwrap();
        navigate('/me/colony');
    };

    const renderSlideCard = (decree: DecreeDetails) => {
        const slide: Slide = {
            id: decree.id.toString(),
            title: decree.name,
            imageName: `pictures/${decree.image}`,
            text: decree.description,
            parameters: [],
            buttons: [],
            footer: undefined
        };
        return <SlideCard slide={slide} closeAction={() => setShowSlide(false)} />;
    };

    const renderButtons = (decree: DecreeDetails) => (
        <div className="flex flex-col gap-3 items-center w-full">
            <YagoButton onClick={() => navigate(-1)} variant="secondary">
                Закрыть
            </YagoButton>
            <YagoButton 
                onClick={() => handleIssueDecree(decree.id)} 
                disabled={!decree.button.isAvailable}
            >
                {decree.button.name}
            </YagoButton>
            <YagoButton onClick={() => setShowSlide(true)} variant="secondary">
                Описание
            </YagoButton>
        </div>
    );

    const renderCard = (decree: DecreeDetails) => (
        <YagoSlide
            title="Указ"
            image={`/assets/images/pictures/${decree.image}.jpg`}
        >
            <div className="flex flex-col gap-4 items-center">
                <YagoCardContentSelection 
                    handlePrev={handlePrevDecree} 
                    label={decree.name} 
                    handleNext={handleNextDecree} 
                />
                <YagoText>
                    {decree.text}
                </YagoText>
                <ColonyParameterList items={decree.parameters} />
                {renderButtons(decree)}
            </div>
        </YagoSlide>
    );

    const renderContent = () => {
        if (decreeResult.data == undefined)
            return;
        return showSlide 
            ? renderSlideCard(decreeResult.data!) 
            : renderCard(decreeResult.data!);
    };

    return (
        <PageContainer backgroundImage='homapage' isLoading={isLoading} error={error}
        >
            {renderContent()}
        </PageContainer>
    );
};

export default DecreePage;