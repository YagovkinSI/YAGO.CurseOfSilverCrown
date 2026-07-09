import vk_logo from '../assets/images/links/vk_logo.svg';
import React from "react";
import YagoSlide from "./YagoSlide";
import SwgWithLink from './SwgWithLink';
import YagoButton from './YagoButton';
import { useNavigate } from 'react-router-dom';

const DefaultErrorCard: React.FC = () => {
    const navigate = useNavigate();

    const renderErrorMessage = () => (
        <p className="text-light/90 text-base mb-4 text-center">
            Произошла ошибка получения данных с сервера. Попробуйте перезагрузить страницу или напишите о проблеме в группе в ВК.
        </p>
    );

    const renderVkLink = () => (
        <div className="flex justify-center mb-4">
            <SwgWithLink 
                url="https://vk.com/club189975977" 
                swgPath={vk_logo} 
                alt="vk link" 
            />
        </div>
    );

    const renderCloseButton = () => (
        <div className="flex justify-center">
            <YagoButton onClick={() => navigate('/')} variant="secondary">
                Закрыть
            </YagoButton>
        </div>
    );

    return (
        <YagoSlide title="Ошибка">
            <div className="flex flex-col gap-3">
                {renderErrorMessage()}
                {renderVkLink()}
                {renderCloseButton()}
            </div>
        </YagoSlide>
    );
};

export default DefaultErrorCard;