<?php

namespace App\Http\Controllers;

use App\Http\Controllers\Controller;
use App\Http\Requests\CreateProjectRequest;
use App\Http\Resources\ProjectResource;
use App\Models\Project;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Storage;
use Illuminate\Support\Str;

class ProjectsController extends Controller
{
    public function __construct()
    {
        $this->middleware(['auth:sanctum'])->except(['index', 'show']);
    }

    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Database\Eloquent\Collection
     */
    public function index()
    {
        return Project::all();
    }

    /**
     * Show the form for creating a new resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function create()
    {
        //
    }

    /**
     * Store a newly created resource in storage.
     *
     * @param  \App\Http\Requests\CreateProjectRequest  $request
     * @return string
     */
    public function store(CreateProjectRequest $request)
    {
        $data = $request->validated();

        $base64_image = $request->input('logo');

        @list($type, $file_data) = explode(';', $base64_image);
        @list(, $file_data) = explode(',', $file_data);

        $imageName = Str::random(10).'.'.'png';

        Storage::disk('public')->put($imageName, base64_decode($file_data));

        $data['logo'] = Storage::disk('public')->url($imageName);

        $data['description'] = json_encode($data['description']);

        return Project::query()
            ->updateOrCreate($data);
    }

    /**
     * Display the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Database\Eloquent\Builder|\Illuminate\Database\Eloquent\Builder[]|\Illuminate\Database\Eloquent\Collection|\Illuminate\Database\Eloquent\Model|\Illuminate\Http\Response
     */
    public function show(int $id)
    {
        return ProjectResource::make(Project::query()
            ->findOrFail($id)
        );
    }

    /**
     * Show the form for editing the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function edit($id)
    {
        //
    }

    /**
     * Update the specified resource in storage.
     *
     * @param  \App\Http\Requests\CreateProjectRequest  $request
     * @param  int  $id
     * @return int
     */
    public function update(CreateProjectRequest $request, $id)
    {
        $data = $request->validated();

        $base64_image = $request->input('logo');

        @list($type, $file_data) = explode(';', $base64_image);
        @list(, $file_data) = explode(',', $file_data);

        $imageName = Str::random(10).'.'.'png';

        Storage::disk('public')->put($imageName, base64_decode($file_data));

        $data['logo'] = Storage::disk('public')->url($imageName);;

        return Project::query()
            ->findOrFail($id)
            ->update($data);
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param int $id
     * @return bool
     * @throws \Throwable
     */
    public function destroy($id)
    {
        return Project::query()
            ->findOrFail($id)
            ->deleteOrFail();
    }
}
