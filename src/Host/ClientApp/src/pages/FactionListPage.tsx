import { Box, FormControl, InputLabel, MenuItem, Paper, Select, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from '@mui/material';
import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import LoadingCard from '../shared/LoadingCard';
import { useGetFactionListQuery, type FactionSortBy } from '../entities/FactionList';
import { useNavigate, useSearchParams } from 'react-router-dom';

type TableParams = {
    page: number;
    pageSize: number;
    sortBy: FactionSortBy;
    sortOrder: 'asc' | 'desc';
};

export const useTableNavigation = () => {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();

    // Получаем текущие параметры с default значениями
    const currentParams: TableParams = {
        page: Number(searchParams.get('page')) || 1,
        pageSize: Number(searchParams.get('pageSize')) || 10,
        sortBy: searchParams.get('sortBy') as FactionSortBy || 'vassalCount',
        sortOrder: (searchParams.get('sortOrder') as 'asc' | 'desc') || 'asc',
    };

    // Функция для обновления параметров
    const updateParams = (newParams: Partial<TableParams>) => {
        const mergedParams = { ...currentParams, ...newParams };
        const queryString = new URLSearchParams({
            page: mergedParams.page.toString(),
            pageSize: mergedParams.pageSize.toString(),
            sortBy: mergedParams.sortBy.toString(),
            sortOrder: mergedParams.sortOrder,
        }).toString();

        navigate(`?${queryString}`, { replace: true });
    };

    return { currentParams, updateParams };
};

const FactionListPage: React.FC = () => {
    const navigate = useNavigate();
    const [searchParams] = useSearchParams();

    const currentParams: TableParams = {
        page: Number(searchParams.get('page')) || 1,
        pageSize: Number(searchParams.get('pageSize')) || 10,
        sortBy: searchParams.get('sortBy') as FactionSortBy || 'vassalCount',
        sortOrder: (searchParams.get('sortOrder') as 'asc' | 'desc') || 'asc',
    };

    // Функция для обновления параметров
    const updateParams = (newParams: Partial<TableParams>) => {
        const mergedParams = { ...currentParams, ...newParams };
        const queryString = new URLSearchParams({
            page: mergedParams.page.toString(),
            pageSize: mergedParams.pageSize.toString(),
            sortBy: mergedParams.sortBy,
            sortOrder: mergedParams.sortOrder,
        }).toString();

        navigate(`?${queryString}`, { replace: true });
    };

    const { data, isLoading, error } = useGetFactionListQuery({
        page: currentParams.page,
        pageSize: currentParams.pageSize,
        sortBy: currentParams.sortBy,
        sortOrder: currentParams.sortOrder == 'desc' ? 'desc' : 'asc'
    });

    const handleSortChange = (newSortBy: FactionSortBy) => {
        updateParams({
            sortBy: newSortBy,
            page: 1,
        });
    };

    const renderCard = () => {
        return (
            <YagoCard
                title={{ name: 'Список фракций' }}
            >
                <Box sx={{ width: '100%' }}>
                    <FormControl fullWidth sx={{ mb: 2 }}>
                        <InputLabel>Отображаемое значение</InputLabel>
                        <Select
                            value={currentParams.sortBy}
                            onChange={(e) => handleSortChange(e.target.value as FactionSortBy)}
                            label="Отображаемое значение"
                        >
                            <MenuItem value="vassalCount">Колличетсво вассалов</MenuItem>
                            <MenuItem value="name">Название</MenuItem>
                            <MenuItem value="warriorCount">Войско</MenuItem>
                            <MenuItem value="gold">Казна</MenuItem>
                            <MenuItem value="investments">Имущество владения</MenuItem>
                            <MenuItem value="fortifications">Укрепления</MenuItem>
                            <MenuItem value="suzerain">Сюзерен</MenuItem>
                            <MenuItem value="user">Пользователь</MenuItem>
                        </Select>
                    </FormControl>
                    <TableContainer component={Paper}>
                        <Table aria-label="responsive table">
                            <TableHead>
                                <TableRow>
                                    <TableCell>№</TableCell>
                                    <TableCell>Название</TableCell>
                                    <TableCell>Значение</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {data!.length > 0 ? (
                                    data!.map((row) => (
                                        <TableRow key={row.entity.id}>
                                            <TableCell>{row.number}</TableCell>
                                            <TableCell>{row.entity.name}</TableCell>
                                            <TableCell>{row.value?.name}</TableCell>
                                        </TableRow>
                                    ))
                                ) : (
                                    <TableRow>
                                        <TableCell colSpan={3} align="center">
                                            Нет данных
                                        </TableCell>
                                    </TableRow>
                                )}
                            </TableBody>
                        </Table>
                    </TableContainer>
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