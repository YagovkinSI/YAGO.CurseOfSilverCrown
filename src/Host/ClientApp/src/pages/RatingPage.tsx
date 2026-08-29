import React, { useRef } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import Page from '../widgets/Page';
import SlideCard from '../widgets/SlideCard';
import Button from '../shared/ui/buttons/Button';
import YagoCardContentSelection from '../widgets/SelectorSlide';
import StatisticRowList from '../entities/statistics/StatisticRowList';
import { useGetRatingsQuery } from '../entities/ratings/ratings.api';
import type { RatingCode } from '../entities/ratings/ratings.types';

const ratingTypes: { code: RatingCode; label: string }[] = [
    { code: 'population', label: 'Население' },
    { code: 'laws', label: 'Законы' },
    { code: 'mood', label: 'Доверие' },
    { code: 'budget', label: 'Бюджет' },
    { code: 'attractiveness', label: 'Привлекательность' },
    { code: 'area', label: 'Занято секторов' },
    { code: 'week', label: 'Ход' },
];

const RatingPage: React.FC = () => {
    const navigate = useNavigate();
    const [searchParams, setSearchParams] = useSearchParams();
    const codeParam = searchParams.get('code');
    const code: RatingCode = ratingTypes.find((t) => t.code === codeParam)?.code ?? 'population';
    const nonceRef = useRef(Date.now());

    const { data, isLoading, error } = useGetRatingsQuery({ code, nonce: nonceRef.current });

    const currentIndex = ratingTypes.findIndex((t) => t.code === code);
    const currentLabel = ratingTypes[currentIndex]?.label ?? '';

    const handleNextRaiting = () => {
        const nextIndex = (currentIndex + 1) % ratingTypes.length;
        setSearchParams({ code: ratingTypes[nextIndex].code });
    };

    const handlePrevRaiting = () => {
        const prevIndex = (currentIndex - 1 + ratingTypes.length) % ratingTypes.length;
        setSearchParams({ code: ratingTypes[prevIndex].code });
    };

    const renderContent = () => {
        if (data == undefined) return;

        return (
            <div className="flex items-center justify-center w-full min-h-full py-2">
                <SlideCard title="Колонии" image={undefined}>
                    <YagoCardContentSelection
                        handlePrev={handlePrevRaiting}
                        label={currentLabel}
                        handleNext={handleNextRaiting}
                    />
                    <StatisticRowList fields={data} showRank />
                    <Button onClick={() => navigate(-1)} variant="secondary">
                        Закрыть
                    </Button>
                </SlideCard>
            </div>
        );
    };

    return (
        <Page backgroundImage="space" isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default RatingPage;
