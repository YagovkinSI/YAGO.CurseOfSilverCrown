import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { useGetMyColonyQuery } from '../entities/MyColony';
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import YagoButton from '../shared/YagoButton';
import { useGetMyCycleQuery, useRunCycleMutation } from '../entities/MyCycle';
import { getRandomWikiPage } from '../features/RandomWikiPage';
import { useGetMyUserQuery } from '../entities/MyUser';
import ColonyParameterList from '../features/ColonyParameterList';
import RowData from '../shared/RowData';
import { AlertCircle } from 'lucide-react';
import { GetColorForQuestType, QuestType } from '../entities/MyQuest';

const MyColonyPage: React.FC = () => {
    const myUserDataResult = useGetMyUserQuery();
    const myColonyResult = useGetMyColonyQuery();
    const myCycleResult = useGetMyCycleQuery();
    const [runCycleMutation, runCycleResult] = useRunCycleMutation();
    const navigate = useNavigate();

    const isLoading = myUserDataResult.isLoading || myColonyResult.isLoading || myCycleResult.isLoading || runCycleResult.isLoading;
    const error = myUserDataResult.error ?? myColonyResult.error ?? myCycleResult.error ?? runCycleResult.error;
    
    const user = myUserDataResult.data?.data;
    const colony = myColonyResult.data?.data;
    const cycle = myCycleResult.data?.data;

    useEffect(() => {
        if (!myUserDataResult.isFetching && myUserDataResult.isSuccess && user == undefined) {
            navigate('/registration');
        }
    }, [myUserDataResult, user, navigate]);

    useEffect(() => {
        if (!myColonyResult.isFetching && myColonyResult.isSuccess && colony != undefined) {
            const autoRunQuest = colony.quests.find(x => x.type == QuestType.Immediately);
            if (autoRunQuest) {
                navigate(`/me/quest/${autoRunQuest.id}`);
            }
        }
    }, [myColonyResult, colony, navigate]);

    const [timeLeft, setTimeLeft] = useState<number>(0);
    const [isReady, setIsReady] = useState<boolean>(false);

    useEffect(() => {
        if (myColonyResult.data?.data == undefined || cycle == undefined) return;

        const updateTimer = () => {
            const startAt = Date.parse(cycle.startAtUtc);
            const now = Date.now();
            const isReady = startAt < Date.now();
            const difference = startAt - now;
            if (isReady || difference <= 0) {
                setIsReady(true);
                setTimeLeft(0);
            } else {
                setIsReady(false);
                setTimeLeft(difference);
            }
        };
        updateTimer();
        const interval = setInterval(updateTimer, 1000);
        return () => clearInterval(interval);
    }, [myColonyResult, cycle]);

    const runCycle = async () => {
        await runCycleMutation().unwrap();
    };

    const openRandomWiki = () => {
        const randomPath = getRandomWikiPage();
        navigate(randomPath);
    };

    const renderQuests = () => {
        const quests = myColonyResult.data!.data!.quests;
        const color = GetColorForQuestType(quests.map(x => x.type));
        return (
            <RowData 
                color={color} 
                icon={AlertCircle} 
                label="События" 
                value={quests.length.toString()} 
                url="/me/quests" 
            />
        );
    };

    const renderContent = () => {
        const colonyParameters = myColonyResult.data!.data!.colonyParameters
            .filter(x => x.parrentType == undefined);
        return (
            <div className="flex flex-col gap-1 w-full max-w-[350px] md:max-w-[700px] mx-auto">
                {renderQuests()}
                <ColonyParameterList items={colonyParameters} />
            </div>
        );
    };

    const formatTime = (milliseconds: number): string => {
        if (milliseconds <= 0) return '00:00';

        const seconds = Math.floor((milliseconds / 1000) % 60);
        const minutes = Math.floor((milliseconds / (1000 * 60)) % 60);

        return `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
    };

    const renderDecreesButton = () => {
        const hasMood = myColonyResult.data!.data!.colonyParameters.find(x => x.type == 'Mood_Total');
        if (!hasMood) return null;

        return (
            <YagoButton onClick={() => navigate('/decree')} type="secondary">
                Указы
            </YagoButton>
        );
    };

    const renderMainButtons = () => {
        if (cycle == undefined) return null;
        
        const isFinish = colony?.newColonyAvailable;
        const buttonText = isReady
            ? 'Завершить ход'
            : `След. ход: ${formatTime(timeLeft)}`;

        return (
            <div className="flex flex-col gap-3 items-center w-full">
                <YagoButton onClick={runCycle} isDisabled={!isReady}>
                    {buttonText}
                </YagoButton>
                <YagoButton onClick={openRandomWiki} type="secondary">
                    Случайная статья
                </YagoButton>
                {isFinish && (
                    <YagoButton onClick={() => navigate('/colony-actions/deactivateColony')} type="delete-warning">
                        Новая колония
                    </YagoButton>
                )}
            </div>
        );
    };

    const renderCard = () => (
        <YagoCard
            title={colony?.name ?? '-'}
            image="/assets/images/pictures/captain_hall.jpg"
        >
            <div className="flex flex-col gap-4 items-center">
                {renderContent()}
                {renderDecreesButton()}
                {renderMainButtons()}
            </div>
        </YagoCard>
    );

    const renderContentWrapper = () => {
        if (isLoading) {
            return <LoadingCard />;
        }
        if (error != undefined) {
            return <DefaultErrorCard />;
        }
        return renderCard();
    };

    return (
        <>
            <ErrorField title="Ошибка" error={error} />
            {renderContentWrapper()}
        </>
    );
};

export default MyColonyPage;