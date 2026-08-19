import SlideCard from '../widgets/SlideCard';
import Button from '../shared/ui/buttons/Button';
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useGetMyColonyQuery } from '../entities/colonies/colony.api';
import YagoCardContentSelection from '../widgets/SelectorSlide';
import Text from '../shared/ui/Text';
import ColonyParameterRowList from '../features/ColonyParameterList';
import Page from '../widgets/Page';
import RequirementParameter from '../entities/events/RequirementParameter';
import { GetParameterIcon } from '../features/GetColonyParameterList';
import ResultSlideRenderer from '../entities/events/ResultSlideRenderer';
import { useGetReformQuery, useIssueReformMutation } from '../entities/reforms/reform.api';
import { type ReformDetails } from '../entities/reforms/reform.types';
import type { ColonyParameter } from '../entities/colonies/colony.types';

const ReformsPage: React.FC = () => {
    const reformCodes = ['Show_1', 'Show_2', 'Show_3', 'Debt']
    const [reformId, setReformId] = useState<number>(1);
    const myColonyResult = useGetMyColonyQuery();
    const reformResult = useGetReformQuery(reformCodes[reformId-1]);
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

    const handleIssueReform = async (reformCode: string) => {
        await issueReform({ reformCode }).unwrap();
        setShowReformResult(true);
    };

    const handleCloseResult = () => setShowReformResult(false);

    const renderButtons = (reform: ReformDetails) => (
        <div className="flex flex-col gap-3 items-center w-full">
            <Button onClick={() => navigate(-1)} variant="secondary">
                Закрыть
            </Button>
            <Button
                onClick={() => handleIssueReform(reform.code)}
                disabled={!reform.button.isAvailable}
            >
                {reform.button.name}
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
                    {reform.description}
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
                {renderCard(reformResult.data!)}
            </div>)
    };

    return (
        <Page backgroundImage='homapage' isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default ReformsPage;