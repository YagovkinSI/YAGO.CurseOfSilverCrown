import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Box, useMediaQuery, useTheme } from '@mui/material';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { WorkspacePremium } from '@mui/icons-material';
import { useGetMyColonyQuery } from '../entities/MyColony';
import React, { useEffect, useState } from 'react';
import StateList from '../shared/StateList';
import { StateItemSolar, type StateItem } from '../entities/StateItem';
import { useNavigate } from 'react-router-dom';
import YagoButton from '../shared/YagoButton';
import { useGetMyCycleQuery, useRunCyrcleMutation } from '../entities/MyCycle';
import isErrorWithStatus from '../shared/ErrorHandler';

const MyColonyPage: React.FC = () => {
    const myColonyResult = useGetMyColonyQuery();
    const myCycleResult = useGetMyCycleQuery();
    const [runCycleMutation, runCycleResult] = useRunCyrcleMutation();

    const isLoading = myColonyResult.isLoading || myCycleResult.isLoading || runCycleResult.isLoading;
    const error = myColonyResult.error ?? myCycleResult.error ?? runCycleResult.error;

    const navigate = useNavigate();
    React.useEffect(() => {
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
    }, [error]);

    useEffect(() => {
        if (myColonyResult.data == undefined || myCycleResult.data?.data == undefined)
            return;

        const updateTimer = () => {
            const isReady = myCycleResult.data!.data!.completedUtc == null;
            const difference = isReady ? 0 : calcDifference(myCycleResult.data!.data!.completedUtc!);
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
        try {
            await runCycleMutation({}).unwrap();
        } catch (error) {
            console.error('Error running cycle:', error);
        }
    }

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    const stats: StateItem[] = [
        {
            icon: WorkspacePremium,
            label: 'Колония',
            value: `${myColonyResult.data?.data?.name}`,
            color: '#9C27B0',
            url: '/state'
        },
        StateItemSolar('Солары', `${myColonyResult.data?.data?.solars} (${myColonyResult.data?.data?.solarsIncome}/ц)`),
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

    const renderBuildingsButton = () => {
        return (
            <YagoButton onClick={() => navigate('/building')} text={'Постройки'} />
        );
    }

    const renderMainButton = () => {
        const buttonText = isReady ? 'Получить доход' : `След. доход: ${formatTime(timeLeft)}`;

        return (
            <YagoButton variant='contained' onClick={runCycle} text={buttonText} isDisabled={!isReady} />
        );
    }

    const renderCard = () => {
        return (
            <YagoCard
                title={myColonyResult.data?.data?.name ?? '-'}
                image={`/assets/images/pictures/captain_hall.jpg`}
            >
                {renderContent()}
                {renderBuildingsButton()}
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