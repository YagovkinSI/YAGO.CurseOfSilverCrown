import React from "react";
import { Skeleton, Typography } from "@mui/material";
import YagoCard from "./YagoCard";

const LoadingCard: React.FC = () => {
    return (
        <YagoCard image={undefined}>
            <Skeleton variant="rounded" height={30} style={{ margin: 'auto' }} />
            <Typography gutterBottom>Подождите. Выполняется загрузка данных с сервера...</Typography>
        </YagoCard>
    )
}

export default LoadingCard