import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import YagoButton from '../shared/YagoButton';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import StateList from '../shared/StateList';
import { useGetColonyRaitingQuery, type ColonyDetails } from '../entities/ColonyDetails';
import { type StateItem, StateItemStyles, StateItemStyleType } from '../entities/StateItem';
import { FormatListNumbered, WorkspacePremium } from '@mui/icons-material';
import YagoCardContentSelection from '../shared/YagoCardContentSelection';

const ColonyRaitingPage: React.FC = () => {
    const navigate = useNavigate();

    const colonyRaitingResult = useGetColonyRaitingQuery({ page: 1 });

    const isLoading = colonyRaitingResult.isLoading;
    const error = colonyRaitingResult.error;

    const raitingTypes = [
        { type: 'Population', label: 'Население' },
        { type: 'GavernorType', label: 'Законы' },
        { type: 'Mood', label: 'Настроение' },
        { type: 'SolarIncome', label: 'Бюджет' },
        { type: 'Attractiveness_Total', label: 'Привлекательность' },
        { type: 'ZonesOccupied', label: 'Занято секторов' },
        { type: 'EpisodeCount', label: 'Сделано ходов' },
    ];

    const [raitingTypeIndex, setRaitingTypeIndex] = useState<number>(0);

    const handleNextRaiting = () => {
        const nextIndex = (raitingTypeIndex + 1) % raitingTypes.length;
        setRaitingTypeIndex(nextIndex);
    };

    const handlePrevRaiting = () => {
        const prevIndex = (raitingTypeIndex - 1 + raitingTypes.length) % raitingTypes.length;
        setRaitingTypeIndex(prevIndex);
    };


    const getRaitingLabel = (raitingType: string): StateItem => {
        let label;
        switch (raitingType) {
            case 'SolarIncome':
                label = StateItemStyles(StateItemStyleType.Solars, 'Колония', 'Бюджет')
                break;
            case 'GavernorType':
                label = StateItemStyles(StateItemStyleType.Laws, 'Колония', 'Законы')
                break;
            case 'Mood':
                label = StateItemStyles(StateItemStyleType.Mood, 'Колония', 'Настроение')
                break;
            case 'Population':
                label = StateItemStyles(StateItemStyleType.Population, 'Колония', 'Население')
                break;
            case 'ZonesOccupied':
                label = StateItemStyles(StateItemStyleType.Zones, 'Колония', 'Занято секторов')
                break;
            case 'EpisodeCount':
                label = StateItemStyles(StateItemStyleType.Unknown, 'Колония', 'Сделано ходов')
                break;
            case 'Attractiveness_Total':
                label = StateItemStyles(StateItemStyleType.Attractiveness, 'Колония', 'Привлекательность')
                break;
        }
        label!.icon = FormatListNumbered;
        return label!;
    };

    const getRaitingItems = (data: ColonyDetails[], raitingType: string): StateItem[] => {

        return data.map(colony => {
            let item;
            switch (raitingType) {
                case 'SolarIncome':
                    item = StateItemStyles(StateItemStyleType.Solars, colony.name, `${colony.colonyParameters.find(x => x.type == 'Economic_Reserves')?.value ?? 0}`)
                    break;
                case 'GavernorType': {
                    item = StateItemStyles(StateItemStyleType.Laws, colony.name, colony.colonyParameters.find(x => x.type == 'Laws_CodeOfLaws')?.value ?? 'Не определены')
                    break; }
                case 'Mood': {
                    item = StateItemStyles(StateItemStyleType.Mood, colony.name, `${colony.colonyParameters.find(x => x.type == 'Mood_Total')?.value ?? 'Не определено'}`)
                    break; }
                case 'Population':
                    item = StateItemStyles(StateItemStyleType.Population, colony.name, `${colony.colonyParameters.find(x => x.type == 'Population_Total')?.value ?? 0} чел.`)
                    break;
                case 'ZonesOccupied':
                    item = StateItemStyles(StateItemStyleType.Zones, colony.name, `${colony.colonyParameters.find(x => x.type == 'AreaCapacity_Occupied')?.value ?? 0}`)
                    break;
                case 'EpisodeCount':
                    item = StateItemStyles(StateItemStyleType.Unknown, colony.name, `${colony.colonyParameters.find(x => x.type == 'EpisodeCount')?.value ?? 0}`)
                    break;
                case 'Attractiveness_Total':
                    item = StateItemStyles(StateItemStyleType.Attractiveness, colony.name,  `${colony.colonyParameters.find(x => x.type == 'Attractiveness_Total')?.value ?? 'Не определено'}`)
                    break;
            }
            item!.icon = WorkspacePremium;

            return item!;
        })
    };

    const renderCard = (data: ColonyDetails[]) => {
        const raitingStats: StateItem[] = [
            getRaitingLabel(raitingTypes[raitingTypeIndex].type),
            ...getRaitingItems(data, raitingTypes[raitingTypeIndex].type)
        ];

        return (
            <YagoCard
                title={'Колонии'}
                image={undefined}
            >
                <YagoCardContentSelection handlePrev={handlePrevRaiting} label={raitingTypes[raitingTypeIndex].label} handleNext={handleNextRaiting} />
                <StateList items={raitingStats} />
                <YagoButton onClick={() => navigate(-1)} type='secondary'>Закрыть</YagoButton>
            </YagoCard>
        )
    }

    return (
        <>
            <ErrorField title='Ошибка' error={error} />
            {isLoading
                ? <LoadingCard />
                : error != undefined || colonyRaitingResult.data?.data == undefined
                    ? <DefaultErrorCard />
                    : renderCard(colonyRaitingResult.data.data)}
        </>
    )
}

export default ColonyRaitingPage