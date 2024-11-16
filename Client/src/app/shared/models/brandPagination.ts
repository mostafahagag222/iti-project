import { IBrand } from './brand';


export interface IBrandPagination {
    pageIndex: number
    pageSize: number
    count: number
    data: IBrand[]
}
