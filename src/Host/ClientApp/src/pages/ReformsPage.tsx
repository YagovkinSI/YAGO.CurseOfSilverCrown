import SlideCard from '../widgets/SlideCard';
import Button from '../shared/ui/buttons/Button';
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useGetMyColonyQuery, useIssueDecreeMutation } from '../entities/colonies/MyColony';
import { useGetDecreeQuery, type DecreeDetails } from '../entities/reforms/DecreeDetails';
import YagoCardContentSelection from '../widgets/SelectorSlide';
import Text from '../shared/ui/Text';
import ColonyParameterRowList from '../features/ColonyParameterList';
import Page from '../widgets/Page';
import type { ColonyParameter } from '../entities/colonies/ColonyParameter';
import RequirementParameter from '../entities/events/RequirementParameter';
import { GetParameterIcon } from '../features/GetColonyParameterList';
import type { Slide } from '../entities/events/Episode';
import ResultSlideRenderer from '../entities/events/ResultSlideRenderer';

const ReformsPage: React.FC = () => {
    const [decreeId, setDecreeId] = useState<number>(1);
    const [showSlide, setShowSlide] = useState<boolean>(false);
    const myColonyResult = useGetMyColonyQuery();
    const decreeResult = useGetDecreeQuery(decreeId);
    const [issueDecree, issueDecreeResult] = useIssueDecreeMutation();
    const [showDecreeResult, setShowDecreeResult] = useState(false);
    const navigate = useNavigate();

    const isLoading = decreeResult.isLoading || myColonyResult.isLoading || issueDecreeResult.isLoading;
    const error = decreeResult.error ?? myColonyResult.error ?? issueDecreeResult.error;
    const decreeIdMax = 4;
    const eventResultSlide = issueDecreeResult.data?.data;

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
        setShowDecreeResult(true);
    };

    const handleCloseResult = () => setShowDecreeResult(false);

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

    const renderSlideCard = (decree: DecreeDetails) => {
        const slide: Slide = {
            id: decree.id.toString(),
            title: decree.name,
            imageName: `pictures/${decree.image}`,
            text: decree.description,
            parameters: [],
            requirements: [],
            buttons: [],
            footer: undefined
        };
        return renderSlide(slide);
    };

    const renderButtons = (decree: DecreeDetails) => (
        <div className="flex flex-col gap-3 items-center w-full">
            <Button onClick={() => navigate(-1)} variant="secondary">
                Закрыть
            </Button>
            <Button
                onClick={() => handleIssueDecree(decree.id)}
                disabled={!decree.button.isAvailable}
            >
                {decree.button.name}
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

    const renderCard = (decree: DecreeDetails) => (
        <SlideCard
            title="Указ"
            image={`/images/pictures//${decree.image}.jpg`}
        >
            <div className="flex flex-col gap-4 items-center">
                <YagoCardContentSelection
                    handlePrev={handlePrevDecree}
                    label={decree.name}
                    handleNext={handleNextDecree}
                />
                <Text>
                    {decree.text}
                </Text>
                {renderRequirements(decree.requirements)}
                <ColonyParameterRowList items={decree.parameters} />
                {renderButtons(decree)}
            </div>
        </SlideCard>
    );

    const renderContent = () => {
        if (decreeResult.data == undefined)
            return;

        if (showDecreeResult && eventResultSlide) {
            return <ResultSlideRenderer eventResult={eventResultSlide} onClose={handleCloseResult} />;
        }

        return (
            <div className="flex flex-l items-center justify-center w-full min-h-full py-2">
                {showSlide
                    ? renderSlideCard(decreeResult.data!)
                    : renderCard(decreeResult.data!)}
            </div>)
    };

    return (
        <Page backgroundImage='homapage' isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default ReformsPage;