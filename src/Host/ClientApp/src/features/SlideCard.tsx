import YagoSlide from '../shared/YagoSlide';
import YagoButton from '../shared/YagoButton';
import React from 'react';
import YagoText from '../shared/YagoText';
import TextFooterComment from '../shared/TextFooterComment';
import type { Slide } from '../entities/Episode';

export interface SlideCardProps {
    slide: Slide,
    closeAction: () => void
};

const SlideCard: React.FC<SlideCardProps> = ({ slide, closeAction }) => {

    return (
            <YagoSlide
                title={slide.title}
                image={`/assets/images/${slide.imageName ?? 'home'}.jpg`}
            >
                <YagoText>
                    {slide.text}
                </YagoText>
                <YagoButton onClick={closeAction} variant='secondary'>Закрыть</YagoButton>
                <TextFooterComment>{slide.footer}</TextFooterComment>
            </YagoSlide>
        )
}

export default SlideCard