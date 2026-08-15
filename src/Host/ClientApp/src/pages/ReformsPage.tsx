import SlideCard from '../widgets/SlideCard';
import Button from '../shared/ui/buttons/Button';
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useGetMyColonyQuery, useIssueReformMutation } from '../entities/colonies/colony.api';
import YagoCardContentSelection from '../widgets/SelectorSlide';
import Text from '../shared/ui/Text';
import ColonyParameterRowList from '../features/ColonyParameterList';
import Page from '../widgets/Page';
import type { ColonyParameter } from '../entities/colonies/ColonyParameter';
import RequirementParameter from '../entities/events/RequirementParameter';
import { GetParameterIcon } from '../features/GetColonyParameterList';
import type { Slide } from '../entities/events/Episode';
import ResultSlideRenderer from '../entities/events/ResultSlideRenderer';
import { useGetReformQuery, type ReformDetails } from '../entities/reforms/ReformDetails';

const ReformsPage: React.FC = () => {
    const [reformId, setReformId] = useState<number>(1);
    const [showSlide, setShowSlide] = useState<boolean>(false);
    const myColonyResult = useGetMyColonyQuery();
    const reformResult = useGetReformQuery(reformId);
    const [issueReform, issueReformResult] = useIssueReformMutation();
    const [showReformResult, setShowReformResult] = useState(false);
    const navigate = useNavigate();

    const isLoading = reformResult.isLoading || myColonyResult.isLoading || issueReformResult.isLoading;
    const error = reformResult.error ?? myColonyResult.error ?? issueReformResult.error;
    const reformIdMax = 4;
    const eventResultSlide = issueReformResult.data?.data;

    useEffect(() => {
        if (myColonyResult.data != undefined && myColonyResult.data.data == undefined) {
            navigate('/');
        }
    }, [myColonyResult, navigate]);

    const handleNextReform = () => {
        const nextIndex = (reformId % reformIdMax) + 1;
        setReformId(nextIndex);
    };

    const handlePrevReform = () => {
        const prevIndex = reformId == 1 ? reformIdMax : reformId - 1;
        setReformId(prevIndex);
    };

    const handleIssueReform = async (reformId: number) => {
        await issueReform({ reformId }).unwrap();
        setShowReformResult(true);
    };

    const handleCloseResult = () => setShowReformResult(false);

    const renderSlide = (slide: Slide) => (
        <SlideCard
            title={slide.title}
            image={`/assets/images/${slide.imageName ?? 'home'}.jpg`}
        >
            <Text>
                {slide.text}
            </Text>
            <Button onClick={() => setShowSlide(false)} variant='secondary'>Закрыть</Button>
            <p className='text-muted text-xs font-light tracking-wide my-2'>
                {slide.footer}
            </p>
        </SlideCard>
    )

    const renderSlideCard = (reform: ReformDetails) => {
        const slide: Slide = {
            id: reform.id.toString(),
            title: reform.name,
            imageName: `pictures/${reform.image}`,
            text: reform.description,
            parameters: [],
            requirements: [],
            buttons: [],
            footer: undefined
        };
        return renderSlide(slide);
    };

    const renderButtons = (reform: ReformDetails) => (
        <div className="flex flex-col gap-3 items-center w-full">
            <Button onClick={() => navigate(-1)} variant="secondary">
                Закрыть
            </Button>
            <Button
                onClick={() => handleIssueReform(reform.id)}
                disabled={!reform.button.isAvailable}
            >
                {reform.button.name}
            </Button>
            <Button onClick={() => setShowSlide(true)} variant="secondary">
                Описание
            </Button>
        </div>
    );

    const renderRequirements = (parameters: ColonyParameter[]) => {
        if (!parameters || parameters.length === 0) return null;
        return <div className='flex flex-col mx-auto w-full gap-0.5'>
            {parameters?.map(parameter => <RequirementParameter
                icon={GetParameterIcon(parameter.type)}
                label={parameter.name}
                value={parameter.value}
                status={parameter.status != 'critical'} />)}
        </div>
    }

    const renderCard = (reform: ReformDetails) => (
        <SlideCard
            title="Указ"
            image={`/images/pictures//${reform.image}.jpg`}
        >
            <div className="flex flex-col gap-4 items-center">
                <YagoCardContentSelection
                    handlePrev={handlePrevReform}
                    label={reform.name}
                    handleNext={handleNextReform}
                />
                <Text>
                    {reform.text}
                </Text>
                {renderRequirements(reform.requirements)}
                <ColonyParameterRowList items={reform.parameters} />
                {renderButtons(reform)}
            </div>
        </SlideCard>
    );

    const renderContent = () => {
        if (reformResult.data == undefined)
            return;

        if (showReformResult && eventResultSlide) {
            return <ResultSlideRenderer eventResult={eventResultSlide} onClose={handleCloseResult} />;
        }

        return (
            <div className="flex flex-l items-center justify-center w-full min-h-full py-2">
                {showSlide
                    ? renderSlideCard(reformResult.data!)
                    : renderCard(reformResult.data!)}
            </div>)
    };

    return (
        <Page backgroundImage='homapage' isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default ReformsPage;