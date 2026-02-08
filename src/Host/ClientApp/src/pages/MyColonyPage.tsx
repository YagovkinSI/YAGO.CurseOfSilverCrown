import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Box, useMediaQuery, useTheme } from '@mui/material';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { useGetMyColonyQuery } from '../entities/MyColony';
import React, { useEffect, useState } from 'react';
import StateList from '../shared/StateList';
import { GavernorTypeStateItem, StateItemStyles, StateItemStyleType, type StateItem } from '../entities/StateItem';
import { useNavigate } from 'react-router-dom';
import YagoButton from '../shared/YagoButton';
import { CycleState, useGetMyCycleQuery } from '../entities/MyCycle';
import isErrorWithStatus from '../shared/ErrorHandler';
import { getRandomWikiPage } from '../features/RandomWikiPage';
import { useGetQuery } from '../entities/MyUser';

const MyColonyPage: React.FC = () => {
    const myUserDataResult = useGetQuery();
    const myColonyResult = useGetMyColonyQuery();
    const myCycleResult = useGetMyCycleQuery();

    const isLoading = myUserDataResult.isLoading || myColonyResult.isLoading || myCycleResult.isLoading;
    const error = myUserDataResult.error ?? myColonyResult.error ?? myCycleResult.error;

    const navigate = useNavigate();

    useEffect(() => {
        if (!myUserDataResult?.data?.isAuthorized) {
            navigate('/registration');
        }
    }, [myUserDataResult, navigate]);

    useEffect(() => {
        if (myColonyResult.data != undefined && myColonyResult.data!.isAuthorized && myColonyResult.data!.data == undefined) {
            navigate('/createColony');
        }
    }, [navigate, myColonyResult]);

    const [timeLeft, setTimeLeft] = useState<number>(0);
    const [isReady, setIsReady] = useState<boolean>(false);

    const calcDifference = (completedUtc: string): number => {
        const completedTime = Date.parse(completedUtc);
        const twoMinutesInMs = 2 * 60 * 1000;
        const targetTime = completedTime + twoMinutesInMs;
        const now = Date.now();
        const difference = targetTime - now;
        return difference;
    }

    useEffect(() => {
        if (error != undefined && isErrorWithStatus(error, 401))
            navigate('/registration');
    }, [error, navigate]);

    useEffect(() => {
        if (myColonyResult.data == undefined || myCycleResult.data?.data == undefined)
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
    }, [myColonyResult, myCycleResult.data, myCycleResult.data?.data]);

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
        StateItemStyles(StateItemStyleType.Colony, 'Колония', myColonyResult.data?.data?.name ?? '-', '/state'),
        GavernorTypeStateItem(myColonyResult.data?.data?.colonyParameters.find(x => x.name == 'Loyalty_Total')?.value ?? 0),
        StateItemStyles(StateItemStyleType.Solars, 'Солары',
            `${myColonyResult.data?.data?.colonyParameters.find(x => x.name == 'Economic_Reserves')?.value ?? 0} 
            (${myColonyResult.data?.data?.colonyParameters.find(x => x.name == 'Economic_Budget_Balance')?.value ?? 0}/ц)`),
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

    const renderUnitsButton = () => {
        const hasWorkers = myColonyResult.data?.data?.colonyParameters.find(x => x.name == 'Population_Total')?.value ?? 0 > 0;

        return (
            <YagoButton variant={hasWorkers ? 'outlined' : 'contained'} onClick={() => navigate('/unit')} text={'Найм'} />
        );
    }

    const renderMainButton = () => {
        const hasWorkers = (myColonyResult.data?.data?.colonyParameters.find(x => x.name == 'Population_Total')?.value ?? 0) > 0;
        const isFinish = (myColonyResult.data?.data?.colonyParameters.find(x => x.name == 'AreaCapacity_Occupied')?.value ?? 0) > 100;

        const buttonText = isReady
            ? myCycleResult.data!.data!.state == CycleState.InProgress
                ? 'Продолжить путь'
                : 'В путь'
            : `След. доход: ${formatTime(timeLeft)}`;

        return (
            <>
                {hasWorkers
                    ? <YagoButton variant='contained' onClick={runCycle} text={buttonText} isDisabled={!isReady} />
                    : <></>}
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
                title={myColonyResult.data?.data?.name ?? '-'}
                image={`/assets/images/pictures/captain_hall.jpg`}
            >
                {renderContent()}
                {renderUnitsButton()}
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