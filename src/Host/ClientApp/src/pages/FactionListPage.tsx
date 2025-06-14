import { Box, Paper, Typography } from '@mui/material';
import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import LoadingCard from '../shared/LoadingCard';
import { useGetFactionListQuery, type FactionSortBy } from '../entities/FactionList';
import { useSearchParams } from 'react-router-dom';

const FactionListPage: React.FC = () => {
    const [searchParams] = useSearchParams();
    const page = Number(searchParams.get('page')) || 1;
    const pageSize = Number(searchParams.get('pageSize')) || 20;
    const sortBy = searchParams.get('sortBy') as FactionSortBy || 'vassalCount';
    const sortOrder = searchParams.get('sortOrder') || 'desc';

    const { data, isLoading, error } = useGetFactionListQuery({ 
        page, 
        pageSize, 
        sortBy, 
        sortOrder: sortOrder == 'desc' ? 'desc' : 'asc'
    });

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