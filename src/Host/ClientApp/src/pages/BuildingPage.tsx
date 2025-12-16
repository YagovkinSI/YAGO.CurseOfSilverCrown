import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import YagoButton from '../shared/YagoButton';
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { StateItemPopulation, StateItemStability, StateItemSolar, StateItemZones, type StateItem } from '../entities/StateItem';
import StateList from '../shared/StateList';
import type { Slide } from '../entities/Slide';
import SlideCard from '../features/SlideCard';
import { useGetMyColonyQuery } from '../entities/MyColony';
import isErrorWithStatus from '../shared/ErrorHandler';
import { useGetBuildingQuery, type BuildingDetails } from '../entities/BuildingDetails';
import YagoCardContentSelection from '../shared/YagoCardContentSelection';
import { useBuyBuildingMutation } from '../entities/ColonyActions';

const BuildingPage: React.FC = () => {
    const navigate = useNavigate();

    const [buildingId, setBuildingId] = useState<number>(1);
    const buildingIdMax = 3;
    const buildingResult = useGetBuildingQuery(buildingId);

    const myColonyResult = useGetMyColonyQuery();
    const [buyBuilding, useBuyBuildingResult] = useBuyBuildingMutation();

    const [showSlide, setShowSlide] = useState<boolean>(false);

    const isLoading = buildingResult.isLoading || myColonyResult.isLoading || useBuyBuildingResult.isLoading;
    const error = buildingResult.error ?? myColonyResult.error ?? useBuyBuildingResult.error;

    useEffect(() => {
        if (error != undefined && isErrorWithStatus(error, 401))
            navigate('/registration');
    }, [error, navigate]);

    const handleNextBuilding = () => {
        const nextIndex = buildingId % buildingIdMax + 1;
        setBuildingId(nextIndex);
    };

    const handlePrevBuilding = () => {
        const prevIndex = buildingId == 1 ? buildingIdMax : buildingId - 1;
        setBuildingId(prevIndex);
    };

    const handleBuyBuilding = async (buildingId: number) => {
        await buyBuilding({ buildingId: buildingId }).unwrap();
        navigate('/me/colony');
    }

    const stats = (building: BuildingDetails): StateItem[] => {
        return [
            StateItemSolar('Цена', `${building.cost}`),
            StateItemZones('Сектора', `${building.zonesOccupied}`),
            StateItemSolar('Доход', `+${building.solarsIncome}/ц`),
            StateItemStability('Стабильность', `${building.stability}`),
            StateItemPopulation('Население', `+${building.population} чел.`),
        ]
    };

    const renderSlideCard = (building: BuildingDetails) => {
        const slide: Slide = {
            id: building.id,
            title: building.name,
            imageName: `buildings/${building.id}`,
            text: building.description,
            footer: undefined
        };

        return (
            <SlideCard slide={slide} closeAction={() => setShowSlide(false)} />
        )
    }

    const renderCard = (building: BuildingDetails) => {
        const isActive = myColonyResult.data?.isAuthorized
            && myColonyResult.data.data != undefined
            && myColonyResult.data.data.solars > building.cost
            && myColonyResult.data.data.zonesTotal - myColonyResult.data.data.zonesOccupied >= building.zonesOccupied

        return (
            <YagoCard
                title='Постройка'
                image={`/assets/images/buildings/${building.id ?? '2'}.jpg`}
            >
                <YagoCardContentSelection handlePrev={handlePrevBuilding} label={building.name} handleNext={handleNextBuilding} />
                <StateList items={stats(building)} />
                <YagoButton onClick={() => navigate(-1)} text={'Закрыть'} isDisabled={false} />
                <YagoButton variant="contained" onClick={() => handleBuyBuilding(building.id)} text={'Купить'} isDisabled={!isActive} />
                <YagoButton onClick={() => setShowSlide(true)} text={'Описание'} />
            </YagoCard>
        )
    }

    return (
        <>
            <ErrorField title='Ошибка' error={error} />
            {isLoading || buildingResult.data == undefined
                ? <LoadingCard />
                : error != undefined
                    ? <DefaultErrorCard />
                    : showSlide
                        ? renderSlideCard(buildingResult.data)
                        : renderCard(buildingResult.data)}
        </>
    )
}

export default BuildingPage