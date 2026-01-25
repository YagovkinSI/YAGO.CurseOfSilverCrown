import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Box, useMediaQuery, useTheme } from '@mui/material';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { WorkspacePremium } from '@mui/icons-material';
import { useGetMyColonyQuery } from '../entities/MyColony';
import React, { useEffect } from 'react';
import StateList from '../shared/StateList';
import { StateItemPopulation, StateItemGavernorType, StateItemShip, StateItemSolar, StateItemZones, type StateItem } from '../entities/StateItem';
import { useNavigate } from 'react-router-dom';
import { ColonyParameterResponseType } from '../entities/ColonyDetails';

const StatePage: React.FC = () => {
    const myColonyResult = useGetMyColonyQuery();

    const isLoading = myColonyResult.isLoading;
    const error = myColonyResult.error;

    const navigate = useNavigate();

    useEffect(() => {
        if (myColonyResult.data != undefined && myColonyResult.data!.isAuthorized && myColonyResult.data!.data == undefined) {
            navigate('/createColony');
        }
    }, [navigate, myColonyResult]);

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    const stats: StateItem[] = [
        {
            icon: WorkspacePremium,
            label: 'Колония',
            value: `${myColonyResult.data?.data?.name}`,
            color: '#9C27B0',
        },
        StateItemGavernorType('Правитель', myColonyResult.data?.data?.colonyParameters[ColonyParameterResponseType.GavernorType] ?? 0),
        StateItemSolar('Солары',
            `${myColonyResult.data?.data?.colonyParameters[ColonyParameterResponseType.Solars]} 
            (${myColonyResult.data?.data?.colonyParameters[ColonyParameterResponseType.SolarsIncome]}/ц)`),
        StateItemShip('Корабль', myColonyResult.data?.data?.colonyParameters[ColonyParameterResponseType.Ship] ?? 1),
        StateItemZones('Сектора',
            `${myColonyResult.data?.data?.colonyParameters[ColonyParameterResponseType.ZonesOccupied]} 
            / ${myColonyResult.data?.data?.colonyParameters[ColonyParameterResponseType.ZonesTotal]}`),
        StateItemPopulation('Население', `${myColonyResult.data?.data?.colonyParameters[ColonyParameterResponseType.Population]}`)
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

    const renderCard = () => {
        return (
            <YagoCard
                title={myColonyResult.data?.data?.name ?? '-'}
                image={`/assets/images/pictures/captain_hall.jpg`}
            >
                {renderContent()}
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