import SlideCard from '../shared/SlideCard';
import Button from '../shared/Button';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useGetColonyRaitingQuery, type ColonyDetails } from '../entities/ColonyDetails';
import YagoCardContentSelection from '../shared/SelectorSlide';
import ColonyParameterRowList from '../features/ColonyParameterList';
import type { ColonyParameter } from '../entities/ColonyParameter';
import PageContainer from '../widgets/ContainerPage';

const ColonyRaitingPage: React.FC = () => {
    const navigate = useNavigate();

    const colonyRaitingResult = useGetColonyRaitingQuery({ page: 1 });

    const isLoading = colonyRaitingResult.isLoading;
    const error = colonyRaitingResult.error;

    const raitingTypes = [
        { type: 'Population', label: 'Население' },
        { type: 'GavernorType', label: 'Законы' },
        { type: 'Mood', label: 'Доверие' },
        { type: 'SolarIncome', label: 'Бюджет' },
        { type: 'Attractiveness_Total', label: 'Привлекательность' },
        { type: 'ZonesOccupied', label: 'Занято секторов' },
        { type: 'CurrentWeek', label: 'Ход' },
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


    const getRaitingLabel = (raitingType: string): ColonyParameter => {
        let label : ColonyParameter;
        switch (raitingType) {
            case 'SolarIncome':
                label = { type:"Economic", name: 'Колония', value: 'Бюджет'}
                break;
            case 'GavernorType':
                label = { type:"Laws_CodeOfLaws", name: 'Колония', value: 'Законы'}
                break;
            case 'Mood':
                label = { type:"Mood_Total", name: 'Колония', value: 'Доверие'}
                break;
            case 'Population':
                label = { type:"Population_Total", name: 'Колония', value: 'Население'}
                break;
            case 'ZonesOccupied':
                label = { type:"AreaCapacity", name: 'Колония', value: 'Занято секторов'}
                break;
            case 'CurrentWeek':
                label = { type:"CurrentWeek", name: 'Колония', value: 'Ход'}
                break;
            case 'Attractiveness_Total':
                label = { type:"Attractiveness_Total", name: 'Колония', value: 'Привлекательность'}
                break;
        }
        return label!;
    };

    const getRaitingItems = (data: ColonyDetails[], raitingType: string): ColonyParameter[] => {

        return data.map(colony => {
            let item : ColonyParameter;
            switch (raitingType) {
                case 'SolarIncome':
                    item = { type: "Economic", name: colony.name, value: `${colony.colonyParameters.find(x => x.type == 'Economic')?.value ?? 0}`}
                    break;
                case 'GavernorType': {
                    item = { type:"Laws_CodeOfLaws", name: colony.name, value: colony.colonyParameters.find(x => x.type == 'Laws_CodeOfLaws')?.value ?? 'Не определены'}
                    break; }
                case 'Mood': {
                    item = { type:"Mood_Total", name: colony.name, value: `${colony.colonyParameters.find(x => x.type == 'Mood_Total')?.value ?? 'Не определено'}`}
                    break; }
                case 'Population':
                    item = { type:"Population_Total", name: colony.name, value: `${colony.colonyParameters.find(x => x.type == 'Population_Total')?.value ?? 0} чел.`}
                    break;
                case 'ZonesOccupied':
                    item = { type:"AreaCapacity", name: colony.name, value: `${colony.colonyParameters.find(x => x.type == 'AreaCapacity')?.value ?? 0}`}
                    break;
                case 'CurrentWeek':
                    item = { type:"CurrentWeek", name: colony.name, value: `${colony.colonyParameters.find(x => x.type == 'CurrentWeek')?.value ?? 0}`}
                    break;
                case 'Attractiveness_Total':
                    item = { type:"Attractiveness_Total", name: colony.name,  value: `${colony.colonyParameters.find(x => x.type == 'Attractiveness_Total')?.value ?? 'Не определено'}`}
                    break;
            }
            return item!;
        })
    };

    const renderContent = () => {
        if (colonyRaitingResult.data == undefined)
            return;
        const data = colonyRaitingResult.data.data;
        const raitingStats: ColonyParameter[] = [
            getRaitingLabel(raitingTypes[raitingTypeIndex].type),
            ...getRaitingItems(data, raitingTypes[raitingTypeIndex].type)
        ];

        return (
            <SlideCard
                title={'Колонии'}
                image={undefined}
            >
                <YagoCardContentSelection handlePrev={handlePrevRaiting} label={raitingTypes[raitingTypeIndex].label} handleNext={handleNextRaiting} />
                <ColonyParameterRowList items={raitingStats} />
                <Button onClick={() => navigate(-1)} variant='secondary'>Закрыть</Button>
            </SlideCard>
        )
    }

    return (
        <PageContainer backgroundImage='space' isLoading={isLoading} error={error}>
            {renderContent()}
        </PageContainer>
    );
}

export default ColonyRaitingPage