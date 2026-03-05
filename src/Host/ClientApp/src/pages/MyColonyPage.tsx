import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Box, useMediaQuery, useTheme } from '@mui/material';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { useGetMyColonyQuery } from '../entities/MyColony';
import React, { useEffect, useState } from 'react';
import StateList from '../shared/StateList';
import { AttractivenessStateItem, MoodTypeStateItem, StateItemStyles, StateItemStyleType, type StateItem } from '../entities/StateItem';
import { useNavigate } from 'react-router-dom';
import YagoButton from '../shared/YagoButton';
import { CycleState, useGetMyCycleQuery } from '../entities/MyCycle';
import { getRandomWikiPage } from '../features/RandomWikiPage';
import { useGetQuery } from '../entities/MyUser';

const MyColonyPage: React.FC = () => {
    const myUserDataResult = useGetQuery();
    const myColonyResult = useGetMyColonyQuery();
    const myCycleResult = useGetMyCycleQuery();

    const isLoading = myUserDataResult.isLoading || myColonyResult.isLoading || myCycleResult.isLoading;
    const error = myUserDataResult.error ?? myColonyResult.error ?? myCycleResult.error;
    const colony = myColonyResult.data?.data;
    const cycle = myCycleResult.data?.data;

    const navigate = useNavigate();

    useEffect(() => {
        if (myColonyResult.data != undefined && myColonyResult.data!.isAuthorized && myColonyResult.data!.data == undefined) {
            navigate('/createColony');
        }
    }, [navigate, myColonyResult]);

    const [timeLeft, setTimeLeft] = useState<number>(0);
    const [isReady, setIsReady] = useState<boolean>(false);

    const calcDifference = (completedUtc: string): number => {
        const completedTime = Date.parse(completedUtc);
        const timeoutInMs = 12 * 1000;
        const targetTime = completedTime + timeoutInMs;
        const now = Date.now();
        const difference = targetTime - now;
        return difference;
    }

    useEffect(() => {
        if (myUserDataResult.data != undefined && !myUserDataResult.data?.isAuthorized)
            navigate('/registration');
    }, [myUserDataResult, navigate]);

    useEffect(() => {
        if (myColonyResult.data?.data == undefined || myCycleResult.data?.data == undefined)
            return;

        const updateTimer = () => {
            const isReady = myCycleResult.data!.data!.state != CycleState.Completed;
            const difference = isReady ? 0 : calcDifference(myCycleResult.data!.data!.runAtUtc!);
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
    }, [myColonyResult, myCycleResult.data]);

    const runCycle = async () => {
        navigate("/colony-actions/runCycle");
    }

    const openRandomWiki = () => {
        const randomPath = getRandomWikiPage();
        navigate(randomPath);
    };

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    const stats: StateItem[] = [
        StateItemStyles(StateItemStyleType.Colony, 'Колония', colony?.name ?? '-', '/state'),
        StateItemStyles(StateItemStyleType.Solars, 'Солары',
            `${colony?.colonyParameters.find(x => x.name == 'Economic_Reserves')?.value ?? 0} 
            (${colony?.colonyParameters.find(x => x.name == 'Economic_Budget_Balance')?.value ?? 0}/ц)`),
        MoodTypeStateItem(colony?.colonyParameters.find(x => x.name == 'Mood_Total')?.value ?? 0, false),
        AttractivenessStateItem(colony?.colonyParameters.find(x => x.name == 'Attractiveness_Extraction')?.value ?? 0, false),
    ];

    const renderContent = () => {
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
                <StateList items={stats} />
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
        return (
            <YagoButton variant='outlined' onClick={() => navigate('/decree')} text={'Указы'} />
        );
    }

    const renderMainButton = () => {
        if (cycle == undefined)
            return <></>;
        const isFinish = (colony?.colonyParameters.find(x => x.name == 'Economic_Budget_Balance')?.value ?? 0) > 150;

        const buttonText = isReady
            ? cycle!.state == CycleState.InProgress
                ? 'Продолжить путь'
                : 'В путь'
            : `След. доход: ${formatTime(timeLeft)}`;

        return (
            <>
                <YagoButton variant='contained' onClick={runCycle} text={buttonText} isDisabled={!isReady} />
                <YagoButton variant='outlined' color='info' onClick={openRandomWiki} text='Случайная статья' />
                {isFinish
                    ? <YagoButton variant='outlined' color='error' onClick={() => navigate('/colony-actions/deactivateColony')} text='Новая колония' />
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