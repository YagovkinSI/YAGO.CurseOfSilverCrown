import { FormControl, InputLabel, Link, MenuItem, Paper, Select, Table, TableBody, TableCell, TableContainer, TableHead, TablePagination, TableRow } from '@mui/material';
import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import LoadingCard from '../shared/LoadingCard';
import { useGetFactionListQuery, type FactionSortBy } from '../entities/FactionList';
import { useNavigate, useSearchParams } from 'react-router-dom';
import type YagoEnity from '../entities/YagoEnity';
import { YagoEntityTypeList } from '../entities/YagoEnity';

type TableParams = {
    page: number;
    sortBy: FactionSortBy;
};

const FactionListPage: React.FC = () => {
    const navigate = useNavigate();
    const [searchParams] = useSearchParams();

    const currentParams: TableParams = {
        page: Number(searchParams.get('page')) || 1,
        sortBy: searchParams.get('sortBy') as FactionSortBy || 'vassalCount',
    };

    const updateParams = (newParams: Partial<TableParams>) => {
        const mergedParams = { ...currentParams, ...newParams };
        const queryString = new URLSearchParams({
            page: mergedParams.page.toString(),
            sortBy: mergedParams.sortBy,
        }).toString();

        navigate(`?${queryString}`, { replace: true });
    };

    const { data, isLoading, error } = useGetFactionListQuery({
        page: currentParams.page,
        sortBy: currentParams.sortBy,
    });

    const handleSortChange = (newSortBy: FactionSortBy) => {
        updateParams({
            sortBy: newSortBy,
            page: 1,
        });
    };

    const renderFormControl = () => {
        return (
            <FormControl fullWidth sx={{ mb: 2 }}>
                <InputLabel>Сортировать по...</InputLabel>
                <Select
                    value={currentParams.sortBy}
                    onChange={(e) => handleSortChange(e.target.value as FactionSortBy)}
                    label="Сортировать по..."
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
        )
    }

    const renderEntity = (entity: YagoEnity) => {
        return (
            entity.type == YagoEntityTypeList.Faction
                ?
                <Link
                    component="span"
                    onClick={() => window.location.href = `/Organizations/Details/${entity.id}`}
                    sx={{
                        textDecoration: 'none',
                        fontWeight: 'bold',
                        color: (theme) => theme.palette.primary.main,
                        cursor: 'pointer',
                        '&:hover': {
                            textDecoration: 'underline'
                        }
                    }}
                >
                    {entity.name}
                </Link >
                : entity.name
        )
    }

    const renderTableBody = (hasValue: boolean) => {
        return (
            <TableBody>
                {data!.items.length > 0 ? (
                    data!.items.map((row) => (
                        <TableRow key={row.entity.id}>
                            <TableCell>{row.number}</TableCell>
                            <TableCell>{renderEntity(row.entity)}</TableCell>
                            {hasValue && <TableCell>{renderEntity(row.value!)}</TableCell>}
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
        )
    }

    const renderTablePagination = () => {
        return (
            <TablePagination
                component="div"
                rowsPerPageOptions={[]}
                count={data!.count}
                rowsPerPage={10}
                page={currentParams.page - 1}
                onPageChange={(_, newPage) => updateParams({ page: newPage + 1 })}
                labelDisplayedRows={({ from, to, count }) =>
                    `${from}-${to} из ${count !== -1 ? count : `больше чем ${to}`}`}
                sx={{
                    '& .MuiTablePagination-selectLabel': {
                        display: 'none'
                    },
                    '& .MuiTablePagination-select': {
                        display: 'none'
                    }
                }}
            />
        )
    }

    const renderTable = () => {
        const hasValue = data!.items.some(i => i.value != undefined);

        return (
            <TableContainer component={Paper}>
                <Table aria-label="responsive table">
                    <TableHead>
                        <TableRow>
                            <TableCell>№</TableCell>
                            <TableCell>Название</TableCell>
                            {hasValue && <TableCell>Значение</TableCell>}
                        </TableRow>
                    </TableHead>
                    {renderTableBody(hasValue)}
                </Table>
            </TableContainer>
        )
    }

    const renderCard = () => {
        return (
            <YagoCard
                title={{ name: 'Список фракций' }}
            >
                {renderFormControl()}
                {renderTable()}
                {renderTablePagination()}
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