import vk_logo from '../assets/images/links/vk_logo.svg'
import React from "react";
import YagoCard from "./YagoCard";
import { Typography } from "@mui/material";
import SwgWithLink from './SwgWithLink';
import ButtonWithLink from './ButtonWithLink';

const DefaultErrorCard: React.FC = () => {
    return (
        <YagoCard image={undefined}>
            <Typography gutterBottom>Произошла ошибка получения данных с сервера. Попробуйте перезагрузить страницу или напишите о проблеме в группе в ВК.</Typography>
            <SwgWithLink url="https://vk.com/club189975977" swgPath={vk_logo} alt="vk link" />
            <ButtonWithLink to={'/app/map/'} text={'Закрыть'} />
        </YagoCard>)
}

export default DefaultErrorCard