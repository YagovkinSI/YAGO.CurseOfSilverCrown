import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Box, useMediaQuery, useTheme } from '@mui/material';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { useGetMyColonyQuery } from '../entities/MyColony';
import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import YagoButton from '../shared/YagoButton';
import ColonyParameterList from '../features/ColonyParameterList';

const StatePage: React.FC = () => {
    const myColonyResult = useGetMyColonyQuery();

    const isLoading = myColonyResult.isLoading;
    const error = myColonyResult.error;

    const navigate = useNavigate();

    useEffect(() => {
        if (myColonyResult.data != undefined && myColonyResult.data!.data == undefined) {
            navigate('/');
        }
    }, [navigate, myColonyResult]);

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

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
                <ColonyParameterList items={myColonyResult.data!.data!.colonyParameters.filter(x => x.parrentType != undefined)} />
            </Box>
        )
    }

    const renderCard = () => {
        return (
            <YagoCard
                title={myColonyResult.data?.data?.name ?? '-'}
                image={`/assets/images/pictures/captain_hall.jpg`}
            >
                {renderContent()}
                <YagoButton onClick={() => navigate(-1)} type='secondary' >Закрыть</YagoButton>
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

export default StatePage