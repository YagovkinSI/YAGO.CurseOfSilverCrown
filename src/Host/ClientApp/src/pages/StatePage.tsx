import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Box, useMediaQuery, useTheme } from '@mui/material';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { WorkspacePremium, Grade, RocketLaunch, People } from '@mui/icons-material';
import type { MyState } from '../entities/MyState';
import React from 'react';
import StateList from '../shared/StateList';
import { StateItemSolar, StateItemZones, type StateItem } from '../entities/StateItem';

const StatePage: React.FC = () => {
    const myState: MyState = {
        id: 0,
        name: '-',
        iserId: 0,
        income: -10,
        solars: 10000,
        reputation: 0,
        population: 0,
        freeZones: 5,
        ship: 'Рассвет-782'
    };

    const isLoading = false;
    const error = undefined;

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    const stats: StateItem[] = [
        {
            icon: WorkspacePremium,
            label: 'Колония',
            value: `${myState.name}`,
            color: '#9C27B0',
        },
        {
            icon: Grade,
            label: 'Репутация',
            value: myState.reputation,
            color: '#4FC3F7',
        },
        StateItemSolar('Солары', `${myState.solars} (${myState.income}/ч)`),                
        {
            icon: RocketLaunch,
            label: 'Корабль',
            value: myState.ship,
            color: '#FF8A65'
        },
        StateItemZones('Зоны', myState.freeZones),
        {
            icon: People,
            label: 'Население',
            value: myState.population,
            color: '#81C784'
        }
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
                title={myState.name}
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