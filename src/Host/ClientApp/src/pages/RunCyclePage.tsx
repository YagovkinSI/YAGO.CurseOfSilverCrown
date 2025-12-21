import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Box, Typography, useMediaQuery, useTheme } from '@mui/material';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import React, { useEffect } from 'react';
import StateList from '../shared/StateList';
import { StateItemSolar, type StateItem } from '../entities/StateItem';
import { useNavigate } from 'react-router-dom';
import YagoButton from '../shared/YagoButton';
import isErrorWithStatus from '../shared/ErrorHandler';
import { ColonyParameterType, useRunCycleMutation } from '../entities/ColonyActions';

const RunCyclePage: React.FC = () => {
    const [runCycleMutation, runCycleResult] = useRunCycleMutation();

    const isLoading = runCycleResult.isLoading;
    const error = runCycleResult.error;

    const navigate = useNavigate();
    React.useEffect(() => {
        runCycleMutation({});
    }, []);

    useEffect(() => {
        if (error != undefined && isErrorWithStatus(error, 401))
            navigate('/registration');
    }, [error, navigate]);

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    const renderText = () => {
        return (
            <Typography textAlign="center" gutterBottom>
                {runCycleResult.data?.notification?.text ?? '-'}
            </Typography>
        )
    }

    const stats: StateItem[] = [
        StateItemSolar(
            'Солары', 
            runCycleResult.data?.notification?.parameters.find(x => x.type == ColonyParameterType.Solars)?.value ?? 0),
    ];

    const renderParameters = () => {
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

    const renderCloseButton = () => {
        return (
            <YagoButton variant='contained' onClick={() => navigate("/me/colony")} text={"Закрыть"} />
        );
    }

    const renderCard = () => {
        return (
            <YagoCard
                title={runCycleResult.data?.notification?.title ?? '-'}
                image={`/assets/images/pictures/captain_hall.jpg`}
            >
                {renderText()}
                {renderParameters()}
                {renderCloseButton()}
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

export default RunCyclePage