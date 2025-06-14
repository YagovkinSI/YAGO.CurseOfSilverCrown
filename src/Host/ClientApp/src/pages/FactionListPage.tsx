import { Box, Paper, Typography } from '@mui/material';
import YagoCard from '../shared/YagoCard';
import { useParams } from 'react-router-dom';
import ErrorField from '../shared/ErrorField';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import LoadingCard from '../shared/LoadingCard';
import { useGetFactionListQuery } from '../entities/FactionList';

const FactionListPage: React.FC = () => {
    const { column } = useParams();
    const columnAsNumber = column == 'up' || column == 'down'
        ? -1
        : column == null
            ? 0
            : parseInt(column, 10) || 0;

    const { data, isLoading, error } = useGetFactionListQuery(columnAsNumber);

    const renderCard = () => {
        return (
            <YagoCard
                title={{ name: 'Список фракций' }}
            >
                <Box>
                    {data!.map((row) => (
                        <Paper key={row.id} sx={{ p: 2, mb: 2 }}>
                            <Typography variant="subtitle1">
                                {row.name}
                            </Typography>
                            <Typography>
                                Войско: {row.warriors}
                            </Typography>
                        </Paper>
                    ))}
                </Box>
            </YagoCard>
        )
    }

    return (
        <>
            <ErrorField title='Ошибка' error={error} />
            {isLoading
                ? <LoadingCard />
                : error == undefined && data != undefined && data != undefined
                    ? renderCard()
                    : <DefaultErrorCard />}
        </>
    )
}

export default FactionListPage