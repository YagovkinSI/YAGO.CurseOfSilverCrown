import vk_logo from '../assets/images/links/vk_logo.svg'
import React from "react";
import YagoCard from "./YagoCard";
import { Typography } from "@mui/material";
import SwgWithLink from './SwgWithLink';
import type YagoLink from '../entities/YagoLink';

const DefaultErrorCard: React.FC = () => {
    const title: YagoLink = { name: 'Ошибка' }

    return (
        <YagoCard title={title} >
            <Typography gutterBottom>Произошла ошибка получения данных с сервера. Попробуйте перезагрузить страницу или напишите о проблеме в группе в ВК.</Typography>
            <SwgWithLink url="https://vk.com/club189975977" swgPath={vk_logo} alt="vk link" />
        </YagoCard>)
}

export default DefaultErrorCard