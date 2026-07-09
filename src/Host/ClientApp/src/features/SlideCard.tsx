import YagoSlide from '../shared/YagoSlide';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import YagoButton from '../shared/YagoButton';
import React from 'react';
import TextMain from '../shared/TextMain';
import TextFooterComment from '../shared/TextFooterComment';
import type { Slide } from '../entities/Episode';

export interface SlideCardProps {
    slide: Slide,
    closeAction: () => void
};

const SlideCard: React.FC<SlideCardProps> = ({ slide, closeAction }) => {
    const isLoading = false;
    const error = undefined;

    const renderCard = () => {
        return (
            <YagoSlide
                title={slide.title}
                image={`/assets/images/${slide.imageName ?? 'home'}.jpg`}
            >
                <TextMain textArray={slide.text}  />
                <YagoButton onClick={closeAction} variant='secondary'>Закрыть</YagoButton>
                <TextFooterComment>{slide.footer}</TextFooterComment>
            </YagoSlide>
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

export default SlideCard