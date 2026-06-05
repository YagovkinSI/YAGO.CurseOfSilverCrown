import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Box, useMediaQuery, useTheme } from '@mui/material';
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
import { PriorityHigh } from '@mui/icons-material';
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
            if (autoRunQuest)
                navigate(`/me/quest/${autoRunQuest.id}`);
        }
    }, [myColonyResult, colony, navigate]);

    const [timeLeft, setTimeLeft] = useState<number>(0);
    const [isReady, setIsReady] = useState<boolean>(false);

    useEffect(() => {
        if (myColonyResult.data?.data == undefined || cycle == undefined)
            return;

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
    }

    const openRandomWiki = () => {
        const randomPath = getRandomWikiPage();
        navigate(randomPath);
    };

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    const renderQuests = () => {
        const quests = myColonyResult.data!.data!.quests;
        const color = GetColorForQuestType(quests.map(x => x.type));
        return (<RowData color={color} icon={PriorityHigh} label={'События'} value={quests.length.toString()} url='/me/quests' />)
    }

    const renderContent = () => {
        const colonyParameters = myColonyResult.data!.data!.colonyParameters
            .filter(x => x.parrentType == undefined);
        return (
            <Box
                display="flex"
                flexDirection="column"
                gap={1}
                sx={{
                    width: '100%',
                    maxWidth: isMobile ? 350 : 700,
                    margin: '0 auto'
                }}
            >
                {renderQuests()}
                <ColonyParameterList items={colonyParameters} />
            </Box>
        )
    }

    const formatTime = (milliseconds: number): string => {
        if (milliseconds <= 0) return '00:00';

        const seconds = Math.floor((milliseconds / 1000) % 60);
        const minutes = Math.floor((milliseconds / (1000 * 60)) % 60);

        return `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
    };

    const renderDecreesButton = () => {
        const hasMood = myColonyResult.data!.data!.colonyParameters.find(x => x.type == 'Mood_Total');
        if (!hasMood)
            return <></>

        return (
            <YagoButton onClick={() => navigate('/decree')} type='secondary'>Указы</YagoButton>
        );
    }

    const renderMainButton = () => {
        if (cycle == undefined)
            return <></>;
        const isFinish = colony?.newColonyAvailable;

        const buttonText = isReady
            ? 'Завершить ход'
            : `След. доход: ${formatTime(timeLeft)}`;

        return (
            <>
                <YagoButton onClick={runCycle} isDisabled={!isReady}>{buttonText}</YagoButton>
                <YagoButton onClick={openRandomWiki} type='secondary'>Случайная статья</YagoButton>
                {isFinish
                    ? <YagoButton onClick={() => navigate('/colony-actions/deactivateColony')} type='delete-warning'>Новая колония</YagoButton>
                    : <></>}
            </>
        );
    }

    const renderCard = () => {
        return (
            <YagoCard
                title={colony?.name ?? '-'}
                image={`/assets/images/pictures/captain_hall.jpg`}
            >
                {renderContent()}
                {renderDecreesButton()}
                {renderMainButton()}
            </YagoCard>
        )
    }

    return (
        <>
            <ErrorField title='Ошибка' error={error} />
            {isLoading
                ? <LoadingCard />
                : error != undefined
                    ? <DefaultErrorCard />
                    : renderCard()}
        </>
    )
}

export default MyColonyPage