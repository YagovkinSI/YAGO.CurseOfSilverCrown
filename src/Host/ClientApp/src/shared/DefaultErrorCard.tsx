import vk_logo from '../assets/images/links/vk_logo.svg'
import React from "react";
import YagoCard from "./YagoCard";
import { Typography } from "@mui/material";
import SwgWithLink from './SwgWithLink';
import YagoButton from './YagoButton';
import { useNavigate } from 'react-router-dom';

const DefaultErrorCard: React.FC = () => {
  const navigate = useNavigate();
  
    return (
        <YagoCard title='Ошибка' >
            <Typography gutterBottom>Произошла ошибка получения данных с сервера. Попробуйте перезагрузить страницу или напишите о проблеме в группе в ВК.</Typography>
            <SwgWithLink url="https://vk.com/club189975977" swgPath={vk_logo} alt="vk link" />
            <YagoButton onClick={() => navigate('/')} type='secondary' >Закрыть</YagoButton>
        </YagoCard>)
}

export default DefaultErrorCard